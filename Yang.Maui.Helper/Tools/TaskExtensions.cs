using System;
using System.Threading;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Tools
{
    /// <summary>
    /// An extension method to safely fire-and-forget a Task or a ValueTask
    /// Ensures the Task will rethrow an Exception if an Exception is caught in IAsyncStateMachine.MoveNext()
    /// Based on <see cref="https://github.com/brminnick/AsyncAwaitBestPractices"/> from Brandon Minnick
    /// </summary>
    public static class TaskExtensions
    {
        internal static async void SafeFireAndForget(this Task task, bool continueOnCapturedContext = false, Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(continueOnCapturedContext);
            }
            catch (Exception ex) when (onException != null)
            {
                onException.Invoke(ex);
            }
        }
    }

    public class CancelableTask
    {
        CancellationTokenSource c;
        public async Task<T> Run<T>(Func<T> action, bool autoCancel, int s = 3)
        {
            if (!autoCancel)
                return await Run<T>(action);
            c = new CancellationTokenSource(1000 * s);
            return await Task.Run(action, c.Token);
        }

        public async void Run(Action action, bool autoCancel, int s = 3)
        {
            if (!autoCancel)
            {
                Run(action);
                return;
            }
            c = new CancellationTokenSource(1000 * s);
            await Task.Run(action, c.Token);
            c.Dispose();
        }

        public async Task<T> Run<T>(Func<T> action)
        {
            c = new CancellationTokenSource();
            return await Task.Run(action, c.Token);
        }

        public async void Run(Action action)
        {
            c = new CancellationTokenSource();
            await Task.Run(action, c.Token);
        }

        public void Cancel()
        {
            try
            {
                c.Cancel();
                c.Dispose();
            }
            finally
            {

            }

        }

    }
}
