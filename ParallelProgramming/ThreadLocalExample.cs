namespace ParallelProgramming;

public class ThreadLocalExample
{
    public void Run()
    {
        var sum = 0;
        Parallel.For(1, 1001,
            () => 0,
            (x, state, threadLocalSum) =>
            {
                threadLocalSum += x;
                Console.WriteLine($"Task - {Task.CurrentId} has sum {threadLocalSum}");
                return threadLocalSum;
            },
            partialSum =>
            {
                Console.WriteLine($"Partial value of task: {Task.CurrentId} is: {partialSum}");
                Interlocked.Add(ref sum, partialSum);
            });

        Console.WriteLine($"Sum of 1..1000 = {sum}");
    }
}