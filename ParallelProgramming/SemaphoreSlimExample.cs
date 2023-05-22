namespace ParallelProgramming;

public class SemaphoreSlimExample
{
    private static readonly SemaphoreSlim Semafore = new SemaphoreSlim(2, 10);

    public void Run()
    {
        for (var i = 0; i < 20; i++)
        {
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Entering Task {Task.CurrentId}");
                Semafore.Wait(); // ReleaseCount--
                Console.WriteLine($"Processing Task {Task.CurrentId}");
            });
        }

        while (Semafore.CurrentCount <= 2)
        {
            Console.WriteLine($"Semaphore Count: {Semafore.CurrentCount}");
            Console.ReadKey();
            Semafore.Release(2);
        }
    }
}