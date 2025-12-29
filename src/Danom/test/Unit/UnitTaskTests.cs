namespace Danom.Tests;

using Xunit;
#pragma warning disable CA1849
public sealed class UnitTaskTests {
    [Fact]
    public async Task ToUnitAsync_Task_CompletesAndReturnsUnit() {
        var t = Task.Delay(10);
        var u = await t.ToUnitAsync();
        Assert.Equal(Unit.Value, u);
    }

    [Fact]
    public async Task ToUnitAsync_Task_ExceptionPropagates() {
        var ex = new InvalidOperationException("boom");
        var t = Task.FromException(ex);

        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(async () => await t.ToUnitAsync());
        Assert.Equal("boom", thrown.Message);
    }

    [Fact]
    public async Task ToUnitAsync_Task_CancellationPropagates() {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var t = Task.Delay(50, cts.Token);

        await Assert.ThrowsAsync<TaskCanceledException>(async () => await t.ToUnitAsync());
    }

    [Fact]
    public async Task ToUnitAsync_ValueTask_CompletesAndReturnsUnit() {
        var vt = new ValueTask(Task.Delay(10));
        var u = await vt.ToUnitAsync();
        Assert.Equal(Unit.Value, u);
    }

    [Fact]
    public async Task ToUnitAsync_ValueTask_ExceptionPropagates() {
        var ex = new InvalidOperationException("boom");
        var vt = new ValueTask(Task.FromException(ex));

        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(async () => await vt.ToUnitAsync());
        Assert.Equal("boom", thrown.Message);
    }

    [Fact]
    public async Task ToUnitAsync_ValueTask_CancellationPropagates() {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var vt = new ValueTask(Task.Delay(50, cts.Token));

        await Assert.ThrowsAsync<TaskCanceledException>(async () => await vt.ToUnitAsync());
    }
}
#pragma warning restore CA1849
