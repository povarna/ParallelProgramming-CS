namespace ParallelProgramming;

public class LinqAggregatorsExample
{
    public void Run()
    {
        var sum = Enumerable.Range(1, 1000).Sum();
        Console.WriteLine($"Sum = {sum}");

        var sum2 = Enumerable.Range(1, 1000)
            .Aggregate(0, (i, acc) => i + acc);

        Console.WriteLine($"Sum = {sum2}");

        var sum3 = ParallelEnumerable
            .Range(1, 1000)
            .Aggregate(0,
                (partialSum, i) => partialSum += i,
                (total, subtotal) => total += subtotal,
                i => i);
        
        Console.WriteLine($"Sum = {sum3}");
    }
}