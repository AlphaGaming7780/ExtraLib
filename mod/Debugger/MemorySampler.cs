using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ExtraLib.Debugger
{
    /// <summary>
    /// Samples the process memory at a fixed interval while some work runs, to report its start, end and peak next
    /// to a <see cref="PerformanceStats"/> report. Native allocations (textures, meshes...) don't show in the managed
    /// heap, so the process working set and private bytes are sampled too. The values are process wide: work
    /// running at the same time elsewhere is included.
    /// Unity's Mono returns 0 for Process.WorkingSet64/PrivateMemorySize64: the native values are read from Windows
    /// directly, and are 0 on other platforms.
    /// </summary>
    public sealed class MemorySampler : IDisposable
    {
        public struct Sample
        {
            /// <summary>Physical memory used by the process (what the Task Manager shows), in bytes.</summary>
            public long WorkingSet;
            /// <summary>Memory committed by the process, in bytes.</summary>
            public long PrivateBytes;
            /// <summary>Managed heap size, in bytes.</summary>
            public long Managed;
        }

        public const int DefaultIntervalMs = 250;

        private readonly Timer m_Timer;
        private readonly object m_Lock = new();

        private Sample m_Start;
        private Sample m_End;
        private Sample m_Peak;
        private bool m_Stopped;

        // Disabled for the session when the API isn't there (other platforms).
        private static bool s_NativeAvailable = true;

        [StructLayout(LayoutKind.Sequential)]
        private struct ProcessMemoryCountersEx
        {
            public uint cb;
            public uint PageFaultCount;
            public UIntPtr PeakWorkingSetSize;
            public UIntPtr WorkingSetSize;
            public UIntPtr QuotaPeakPagedPoolUsage;
            public UIntPtr QuotaPagedPoolUsage;
            public UIntPtr QuotaPeakNonPagedPoolUsage;
            public UIntPtr QuotaNonPagedPoolUsage;
            public UIntPtr PagefileUsage;
            public UIntPtr PeakPagefileUsage;
            public UIntPtr PrivateUsage;
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", EntryPoint = "K32GetProcessMemoryInfo", SetLastError = true)]
        private static extern bool GetProcessMemoryInfo(IntPtr process, out ProcessMemoryCountersEx counters, uint size);

        public Sample Start { get { lock (m_Lock) return m_Start; } }
        /// <summary>The last sample, or the final one once stopped.</summary>
        public Sample End { get { lock (m_Lock) return m_End; } }
        /// <summary>The highest value seen for each field (not necessarily in the same sample).</summary>
        public Sample Peak { get { lock (m_Lock) return m_Peak; } }

        /// <summary>
        /// Takes the start sample and starts sampling every <paramref name="intervalMs"/> milliseconds. A peak shorter
        /// than the interval can be missed.
        /// </summary>
        public MemorySampler(int intervalMs = DefaultIntervalMs)
        {
            m_Start = m_Peak = m_End = TakeSample();
            m_Timer = new Timer(_ => Record(), null, intervalMs, intervalMs);
        }

        /// <summary>
        /// Reads the current process memory once.
        /// </summary>
        public static Sample TakeSample()
        {
            Sample sample = new() { Managed = GC.GetTotalMemory(false) };

            if (!s_NativeAvailable) return sample;

            // Profiling only: the caller must never fail because the memory couldn't be read.
            try
            {
                if (GetProcessMemoryInfo(GetCurrentProcess(), out ProcessMemoryCountersEx counters, (uint)Marshal.SizeOf<ProcessMemoryCountersEx>()))
                {
                    sample.WorkingSet = (long)counters.WorkingSetSize.ToUInt64();
                    sample.PrivateBytes = (long)counters.PrivateUsage.ToUInt64();
                }
            }
            catch (Exception e)
            {
                s_NativeAvailable = false;
                EL.Logger.Warn($"Couldn't read the process memory, only the managed memory will be reported: {e.Message}");
            }

            return sample;
        }

        private void Record()
        {
            lock (m_Lock)
            {
                if (m_Stopped) return;

                Sample sample = TakeSample();
                m_End = sample;
                m_Peak.WorkingSet = Math.Max(m_Peak.WorkingSet, sample.WorkingSet);
                m_Peak.PrivateBytes = Math.Max(m_Peak.PrivateBytes, sample.PrivateBytes);
                m_Peak.Managed = Math.Max(m_Peak.Managed, sample.Managed);
            }
        }

        /// <summary>
        /// Stops sampling (taking a last sample) and formats the result as a line for a <see cref="PerformanceStats"/>
        /// report. Calling it again returns the same result.
        /// </summary>
        public string Stop()
        {
            Record();
            lock (m_Lock) m_Stopped = true;
            m_Timer.Dispose();
            return BuildReport();
        }

        /// <summary>
        /// Formats the start, end and peak values, without stopping.
        /// </summary>
        public string BuildReport()
        {
            Sample start = Start, end = End, peak = Peak;
            return $"  Memory (process, GB, start → end / peak): working set {ToGB(start.WorkingSet)} → {ToGB(end.WorkingSet)} / {ToGB(peak.WorkingSet)}"
                + $", private {ToGB(start.PrivateBytes)} → {ToGB(end.PrivateBytes)} / {ToGB(peak.PrivateBytes)}"
                + $", managed {ToGB(start.Managed)} → {ToGB(end.Managed)} / {ToGB(peak.Managed)}";
        }

        public void Dispose()
        {
            lock (m_Lock) m_Stopped = true;
            m_Timer.Dispose();
        }

        private static string ToGB(long bytes) => (bytes / (1024.0 * 1024.0 * 1024.0)).ToString("F2");
    }
}
