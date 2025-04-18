namespace Danom;

/// <summary>
/// Contains extension methods for <see cref="Unit"/> that allow for converting
/// between <see cref="Unit"/> and <see cref="Task"/>.
/// </summary>
public static class UnitTaskExtensions
{
    /// <summary>
    /// Converts the specified task to a task that returns <see cref="Unit"/>.
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    public static async Task<Unit> ToUnitAsync(this Task task)
    {
        await task;
        return Unit.Value;
    }
}
