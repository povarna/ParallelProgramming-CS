namespace ParallelProgramming;

public class LinqMergeOperations
{
    public void Run()
    {
        var numbers = Enumerable.Range(1, 20).ToArray();

        // AsParallel works like producer / consumer pattern because the collection is lazy evaluated and just only when we run foreach we eagerly evaluate the collection. 
        var results = numbers.AsParallel()
            .WithMergeOptions(ParallelMergeOptions.NotBuffered) // this will tell how the merge is done from chunks. 
            .Select(x =>
        {
            var partialResult = Math.Log10(x);
            Console.Write($"P {partialResult}\t");
            return partialResult;
        });
        
        foreach (var result in results)
        {
            Console.Write($"C {result} \t");
        }
    }
}