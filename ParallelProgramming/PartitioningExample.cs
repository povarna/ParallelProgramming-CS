using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;

namespace ParallelProgramming;

public class PartitioningExample
{
    public void SquireEachValue()
    {
        const int count = 1000;
        var values = Enumerable.Range(0, count);
        var result = new int[count];

        Parallel.ForEach(values, x => { result[x] = (int)Math.Pow(x, 2); });
    }

    public void SquireEachChunked()
    {
        const int count = 100000;
        var results = new int[count];

        var part = Partitioner.Create(0, count, 10000);
        Parallel.ForEach(part, range =>
        {
            for (var i = range.Item1; i < range.Item2; i++)
            {
                results[i] = (int)Math.Pow(i, 2);
            }
        });
    }
}