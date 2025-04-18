using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CSharpFunctionalExtensions;
using Danom;
using OneOf;
using OneOf.Types;

#pragma warning disable CA1822 // Mark members as static

static class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<MemoryBenchmark>();
    }
}

[MemoryDiagnoser]
public class MemoryBenchmark
{
    private const int Iterations = 100;

    [Benchmark(Baseline = true)]
    public void Nullable()
    {
        for (var i = 0; i < Iterations; i++)
        {
            var x = new int?(i);
            var _ = x is int y ? y % 2 : 0;
        }
    }

    [Benchmark]
    public void CSharpFunctionalExtensions()
    {
        for (var i = 0; i < Iterations; i++)
        {
            var _ = Maybe<int>.From(i).Match(i => i % 2, () => 0);
        }
    }

    [Benchmark]
    public void Danom()
    {
        for (var i = 0; i < Iterations; i++)
        {
            var _ = Option<int>.Some(i).Match(i => i % 2, () => 0);
        }
    }

    [Benchmark]
    public void OneOf()
    {
        for (var i = 0; i < Iterations; i++)
        {
            var _ = OneOf<int, None>.FromT0(i).Match(i => i % 2, _ => 0);
        }
    }
}
