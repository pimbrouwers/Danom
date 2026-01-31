#pragma warning disable CA1849 // Prefer the 'ValueTask' type for performance
#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable CA2008 // Do not create tasks without passing a TaskScheduler
using Xunit;

namespace Danom.Tests;

public class TaskExtensionsTests {
    [Fact]
    public async Task WaitOrCancel_Task_CompletesSuccessfully() {
        var task = Task.FromResult(42);
        var result = await task.WaitOrCancel();
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task WaitOrCancel_Task_CanceledBeforeCompletion_ThrowsOperationCanceledException() {
        var cts = new CancellationTokenSource();
        var task = Task.Delay(1000).ContinueWith(_ => 42);

        cts.Cancel(); // Cancel immediately

        await Assert.ThrowsAsync<OperationCanceledException>(() => task.WaitOrCancel(cts.Token));
    }

    [Fact]
    public async Task WaitOrCancel_Task_NotCanceled_CompletesSuccessfully() {
        var cts = new CancellationTokenSource();
        var task = Task.FromResult(42);

        var result = await task.WaitOrCancel(cts.Token);
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task WaitOrCancel_ValueTask_CompletesSuccessfully() {
        var valueTask = new ValueTask<int>(42);
        var result = await valueTask.WaitOrCancel();
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task WaitOrCancel_ValueTask_CanceledBeforeCompletion_ThrowsOperationCanceledException() {
        var cts = new CancellationTokenSource();
        var valueTask = new ValueTask<int>(Task.Delay(1000).ContinueWith(_ => 42));

        cts.Cancel(); // Cancel immediately

        await Assert.ThrowsAsync<OperationCanceledException>(() => valueTask.WaitOrCancel(cts.Token));
    }

    [Fact]
    public async Task WaitOrCancel_ValueTask_NotCanceled_CompletesSuccessfully() {
        var cts = new CancellationTokenSource();
        var valueTask = new ValueTask<int>(42);

        var result = await valueTask.WaitOrCancel(cts.Token);
        Assert.Equal(42, result);
    }
}
# pragma warning restore CA2000 // Dispose objects before losing scope
# pragma warning restore CA1849 // Prefer the 'ValueTask' type for performance
# pragma warning restore CA2008 // Do not create tasks without passing a TaskScheduler
