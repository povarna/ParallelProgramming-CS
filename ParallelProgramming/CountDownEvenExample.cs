namespace ParallelProgramming;

public class CountDownEvenExample
{
    private const int TaskCount = 5;
    private static readonly Random Random = new();
    private static readonly CountdownEvent Cte = new(TaskCount);

    public void Run()
    {
        for (var i = 0; i < TaskCount; i++)
        {
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Entering task {Task.CurrentId}");
                Thread.Sleep(Random.Next(3000));
                Cte.Signal();
                Console.WriteLine($"Exiting task {Task.CurrentId}");
            });
        }

        var finalTask = Task.Factory.StartNew(() =>
        {
            Console.WriteLine($"waiting for other tasks to complete in {Task.CurrentId}");
            Cte.Wait();
            Console.WriteLine("All task completed");
        });

        finalTask.Wait();
    }
}