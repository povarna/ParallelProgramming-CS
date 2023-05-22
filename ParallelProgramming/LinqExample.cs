namespace ParallelProgramming;

public class LinqExample
{
    /*
     * Linq is single thread by default
     */
    public void ProcessInAnyOrder()
    {
        const int count = 50;
        var items = Enumerable.Range(1, count).ToArray();
        var results = new int[count];

        items.AsParallel()
            .ForAll(x =>
            {
                var newValue = x * x * x;
                Console.WriteLine($"{newValue} ({Task.CurrentId})");
                // Array starts with index 0 where the range starts from 1
                results[x - 1] = newValue;
            });
        Console.WriteLine();

        foreach (var result in results)
            Console.WriteLine($"{result}\t");

        Console.WriteLine();
    }

    public void ProcessItemsInSequence()
    {
        const int count = 50;
        var items = Enumerable.Range(1, count).ToArray();
        
        var cubes = items.AsParallel().AsOrdered()
            .Select(x => x * x * x);

        foreach (var cube in cubes)
            Console.Write($"{cube}\t");
        Console.WriteLine();
    }
}