using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Colossal.Core;
using ExtraLib.Debugger;

namespace ExtraLib.Helpers
{
    /// <summary>
    /// Thread-safe wrappers around Colossal's MainThreadDispatcher.
    /// Note: actions dispatched from a background thread are stored in a ConcurrentDictionary by the game, so
    /// they run in NO guaranteed order relative to each other. If order matters, wait on the returned Task
    /// before dispatching the next one (or do all the work in a single dispatched call).
    /// Profiling: when a <see cref="PerformanceStats"/> is current on the calling thread, every call is recorded in it:
    /// "MainThread.QueueLatency" (dispatch → start on the main thread, background callers only — roughly what a
    /// caller blocking on the Task waits), "MainThread.Run" (time spent running on the main thread) and the
    /// "MainThread.Dispatched"/"MainThread.Inline" counters.
    /// </summary>
    public static class MainThreadHelper
    {
        /// <summary>
        /// True when called from the main thread. Use it to avoid deadlocks: never block the main thread
        /// (.Wait()/.Result) on a Task that itself needs the main thread to complete.
        /// </summary>
        public static bool IsMainThread => MainThreadDispatcher.isMainThread;

        /// <summary>
        /// Schedules <paramref name="func"/> to run on the main thread and returns a Task for its result.
        /// Use this to safely call main-thread-only Unity APIs (e.g. ScriptableObject.CreateInstance,
        /// PrefabsHelper) from a background thread/Task. The caller decides whether to block on the result
        /// right away (.Result/.Wait()) or keep doing other work and check on it later.
        /// When already on the main thread, <paramref name="func"/> runs inline and the returned Task is completed.
        /// </summary>
        public static Task<T> RunOnMainThread<T>(Func<T> func) => Dispatch(func);

        // Single implementation behind every overload. Not an overload itself: a lambda like `() => { ...; return null; }`
        // also converts to Func<Task<T>>, and picking that overload would Unwrap a null Task into a canceled one.
        private static Task<T> Dispatch<T>(Func<T> func)
        {
            // RunContinuationsAsynchronously: the caller's continuations must not end up running on the main thread.
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

            // Captured on the calling thread: the stats current on the main thread are unrelated.
            PerformanceStats stats = PerformanceStats.Current;
            bool inline = IsMainThread;
            long dispatchedAt = stats != null ? Stopwatch.GetTimestamp() : 0;
            stats?.Increment(inline ? "MainThread.Inline" : "MainThread.Dispatched");

            MainThreadDispatcher.RunOnMainThread(() =>
            {
                long startedAt = stats != null ? Stopwatch.GetTimestamp() : 0;
                if (stats != null && !inline) stats.Record("MainThread.QueueLatency", startedAt - dispatchedAt);

                try
                {
                    T result = func();
                    stats?.Record("MainThread.Run", Stopwatch.GetTimestamp() - startedAt);
                    tcs.SetResult(result);
                }
                catch (Exception e)
                {
                    stats?.Record("MainThread.Run", Stopwatch.GetTimestamp() - startedAt);
                    tcs.SetException(e);
                }
            });

            return tcs.Task;
        }

        /// <summary>
        /// Schedules <paramref name="action"/> to run on the main thread and returns a Task that completes
        /// once it has run, faulting with the original exception if <paramref name="action"/> threw.
        /// Unlike MainThreadDispatcher.RunOnMainThread(Action), which is fire-and-forget and silently
        /// swallows exceptions, this lets the caller observe completion and failures.
        /// </summary>
        public static Task RunOnMainThread(Action action)
        {
            return Dispatch<object>(() =>
            {
                action();
                return null;
            });
        }

        /// <summary>
        /// Starts <paramref name="func"/> on the main thread and returns a Task that completes when the Task it
        /// returned completes (only the start is bound to the main thread).
        /// </summary>
        public static Task RunOnMainThread(Func<Task> func)
        {
            return Dispatch(func).Unwrap();
        }

        /// <summary>
        /// Same as <see cref="RunOnMainThread(Func{Task})"/>, with a result.
        /// </summary>
        public static Task<T> RunOnMainThread<T>(Func<Task<T>> func)
        {
            return Dispatch(func).Unwrap();
        }

        /// <summary>
        /// Thread-safe replacement for MainThreadDispatcher.WaitXFrames, which adds to a plain List that the main
        /// thread iterates every frame and therefore must not be called from a background thread.
        /// Never block the main thread on the returned Task: it would never complete.
        /// </summary>
        public static Task WaitFrames(int frames)
        {
            if (frames <= 0) return Task.CompletedTask;

            return RunOnMainThread(() => MainThreadDispatcher.WaitXFrames(frames));
        }
    }
}
