namespace Danom {
    using System.Threading;
    using System.Threading.Tasks;

    internal static class TaskExtensions {
        internal static async Task<T> WaitOrCancel<T>(this Task<T> task, CancellationToken? token = null) {
            if (token is CancellationToken t) {
                t.ThrowIfCancellationRequested();
                await Task.WhenAny(task, t.WhenCanceled());
                t.ThrowIfCancellationRequested();
            }

            return await task;
        }

        internal static Task<bool> WhenCanceled(this CancellationToken cancellationToken) {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s!).SetResult(true), tcs);
            return tcs.Task;
        }
    }

    internal static class ValueTaskExtensions {
        internal static async Task<T> WaitOrCancel<T>(this ValueTask<T> valueTask, CancellationToken? token = null) {
            if (token is CancellationToken t) {
                t.ThrowIfCancellationRequested();
                var task = valueTask.AsTask();
                await Task.WhenAny(task, t.WhenCanceled());
                t.ThrowIfCancellationRequested();
                return await task;
            }

            return await valueTask;
        }
    }
}
