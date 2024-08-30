using Danom;

public static class OptionSamples
{
    private static readonly IEnumerable<int> _store = Enumerable.Range(1, 100);

    public static Option<int> TryFindInt(Func<int, bool> predicate) =>
        _store.FirstOrDefault(predicate).ToOption();

    public static void Test()
    {
        if (Option<int>.Some(1).DefaultValue(1) == 1)
        {
            Console.WriteLine("Some");
        }
    }
}

public static class Program
{
    public static void Main()
    {
        OptionSamples.Test();
    }
}
