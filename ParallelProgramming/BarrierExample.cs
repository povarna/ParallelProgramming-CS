namespace ParallelProgramming;

public class BarrierExample
{
    private static readonly Barrier Barrier =
        new Barrier(2, b => { Console.WriteLine($"Phase {b.CurrentPhaseNumber} is finished"); });

    private static void Water()
    {
        Console.WriteLine("Putting the kettle on (takes a bit longer)");
        Thread.Sleep(2000);
        Barrier.SignalAndWait();
        Console.WriteLine("Pouring water into the cup");
        Barrier.SignalAndWait();
        Console.WriteLine("Putting the kettle away");
    }

    private static void Cup()
    {
        Console.WriteLine("Finding the nicest cup of tea (fast)");
        Barrier.SignalAndWait();
        Console.WriteLine("Adding tea.");
        Barrier.SignalAndWait();
        Console.WriteLine("Adding sugar");
    }

    public void Run()
    {
        var water = Task.Factory.StartNew(Water);
        var cup = Task.Factory.StartNew(Cup);

        var tea = Task.Factory.ContinueWhenAll(
            new[] { water, cup }, _ =>
            {
                Console.WriteLine("Enjoy your cup of tea"); 
                
            });
        tea.Wait();
    }
}