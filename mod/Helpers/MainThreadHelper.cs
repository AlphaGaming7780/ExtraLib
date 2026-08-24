using System;
using System.Threading.Tasks;
using Colossal.Core;

namespace ExtraLib.Helpers
{
    public static class MainThreadHelper
    {
        /// <summary>
        /// Schedules <paramref name="func"/> to run on the main thread and returns a Task for its result.
        /// Use this to safely call main-thread-only Unity APIs (e.g. ScriptableObject.CreateInstance,
        /// PrefabsHelper) from a background thread/Task. The caller decides whether to block on the result
        /// right away (.Result/.Wait()) or keep doing other work and check on it later.
        /// </summary>
        public static Task<T> RunOnMainThread<T>(Func<T> func)
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

            MainThreadDispatcher.RunOnMainThread(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
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
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            MainThreadDispatcher.RunOnMainThread(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(null);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });

            return tcs.Task;
        }
    }
}
