using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ExtraLib.Debugger
{
    /// <summary>
    /// Thread-safe timing aggregator, for profiling work spread over several threads (Colossal.PerformanceCounter
    /// only times a single scope). Each named section accumulates a call count, a total and a max duration; counters
    /// only accumulate a value. <see cref="BuildReport"/> formats everything as a table for the log.
    /// Stats can have a parent: everything recorded is also recorded in the parent (e.g. per importer → whole pass).
    /// </summary>
    public sealed class PerformanceStats
    {
        private sealed class Section
        {
            public long Count;
            public long TotalTicks;
            public long MaxTicks;
        }

        // Stats of the code running on this thread, see MakeCurrent. MainThreadHelper records its main-thread
        // round-trips into the stats current on the dispatching thread.
        [ThreadStatic] private static PerformanceStats s_Current;

        private readonly ConcurrentDictionary<string, Section> m_Sections = new();
        private readonly ConcurrentDictionary<string, Section> m_Counters = new();
        private readonly Stopwatch m_Stopwatch = Stopwatch.StartNew();

        public string Name { get; }
        public PerformanceStats Parent { get; }
        public TimeSpan Elapsed => m_Stopwatch.Elapsed;
        public bool IsEmpty => m_Sections.IsEmpty && m_Counters.IsEmpty;

        public static PerformanceStats Current => s_Current;

        public PerformanceStats(string name, PerformanceStats parent = null)
        {
            Name = name;
            Parent = parent;
        }

        /// <summary>
        /// Times the scope into <paramref name="section"/>: <c>using (stats.Measure("Load")) { ... }</c>.
        /// </summary>
        public Scope Measure(string section) => new Scope(this, section);

        /// <summary>
        /// Same as <see cref="Measure"/> on <see cref="Current"/>; does nothing when no stats are current.
        /// </summary>
        public static Scope MeasureCurrent(string section) => new Scope(s_Current, section);

        public static void IncrementCurrent(string counter, long amount = 1) => s_Current?.Increment(counter, amount);

        /// <summary>
        /// Makes these stats <see cref="Current"/> on this thread until the returned scope is disposed.
        /// </summary>
        public CurrentScope MakeCurrent() => new CurrentScope(this);

        public void Record(string section, TimeSpan elapsed) => Record(section, (long)(elapsed.TotalSeconds * Stopwatch.Frequency));

        /// <param name="stopwatchTicks">Duration in <see cref="Stopwatch"/> ticks (see <see cref="Stopwatch.GetTimestamp"/>).</param>
        public void Record(string section, long stopwatchTicks)
        {
            Section data = m_Sections.GetOrAdd(section, _ => new Section());
            Interlocked.Increment(ref data.Count);
            Interlocked.Add(ref data.TotalTicks, stopwatchTicks);

            long max = Volatile.Read(ref data.MaxTicks);
            while (stopwatchTicks > max)
            {
                long previous = Interlocked.CompareExchange(ref data.MaxTicks, stopwatchTicks, max);
                if (previous == max) break;
                max = previous;
            }

            Parent?.Record(section, stopwatchTicks);
        }

        public void Increment(string counter, long amount = 1)
        {
            Interlocked.Add(ref m_Counters.GetOrAdd(counter, _ => new Section()).Count, amount);
            Parent?.Increment(counter, amount);
        }

        public void Reset()
        {
            m_Sections.Clear();
            m_Counters.Clear();
            m_Stopwatch.Restart();
        }

        /// <summary>
        /// Sections sorted by total time. Sections overlap (nested scopes, several threads), so totals don't add up
        /// to the elapsed time.
        /// </summary>
        public string BuildReport()
        {
            StringBuilder builder = new();
            builder.AppendLine($"[Perf] {Name} - {Elapsed.TotalSeconds:F3}s elapsed");

            if (!m_Sections.IsEmpty)
            {
                int nameWidth = Math.Max(8, m_Sections.Keys.Max(key => key.Length));
                builder.AppendLine($"  {"Section".PadRight(nameWidth)} {"Calls",8} {"Total ms",11} {"Avg ms",9} {"Max ms",9}");

                foreach (var section in m_Sections.OrderByDescending(pair => Volatile.Read(ref pair.Value.TotalTicks)))
                {
                    long count = Volatile.Read(ref section.Value.Count);
                    double totalMs = ToMilliseconds(Volatile.Read(ref section.Value.TotalTicks));
                    double maxMs = ToMilliseconds(Volatile.Read(ref section.Value.MaxTicks));
                    double avgMs = count > 0 ? totalMs / count : 0;
                    builder.AppendLine($"  {section.Key.PadRight(nameWidth)} {count,8} {totalMs,11:F1} {avgMs,9:F2} {maxMs,9:F1}");
                }
            }

            if (!m_Counters.IsEmpty)
            {
                builder.Append("  Counters: ");
                builder.AppendLine(string.Join(", ", m_Counters.OrderBy(pair => pair.Key).Select(pair => $"{pair.Key}={Volatile.Read(ref pair.Value.Count)}")));
            }

            return builder.ToString().TrimEnd();
        }

        private static double ToMilliseconds(long stopwatchTicks) => stopwatchTicks * 1000.0 / Stopwatch.Frequency;

        public readonly struct Scope : IDisposable
        {
            private readonly PerformanceStats m_Stats;
            private readonly string m_Section;
            private readonly long m_Start;

            internal Scope(PerformanceStats stats, string section)
            {
                m_Stats = stats;
                m_Section = section;
                m_Start = stats != null ? Stopwatch.GetTimestamp() : 0;
            }

            public void Dispose()
            {
                m_Stats?.Record(m_Section, Stopwatch.GetTimestamp() - m_Start);
            }
        }

        public readonly struct CurrentScope : IDisposable
        {
            private readonly PerformanceStats m_Previous;

            internal CurrentScope(PerformanceStats stats)
            {
                m_Previous = s_Current;
                s_Current = stats;
            }

            public void Dispose()
            {
                s_Current = m_Previous;
            }
        }
    }
}
