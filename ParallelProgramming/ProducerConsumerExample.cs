using System.Collections.Concurrent;

namespace ParallelProgramming;

public static class ProducerConsumerExample
{
    public static readonly CancellationTokenSource Cts = new ();
    private static readonly BlockingCollection<int> Messages = new(new ConcurrentBag<int>(), 100);
    private static readonly Random RandomGenerator = new();
    

    public static void ProduceAndConsume()
    {
        var producer = Task.Factory.StartNew(RunProducer);
        var consumer = Task.Factory.StartNew(RunConsumer);

        try
        {
            Task.WaitAll(new[] { producer, consumer }, Cts.Token);
        }
        catch (AggregateException aggregateException)
        {
            // We expect exceptions caused to cancellation
            aggregateException.Handle(e => true);
        }
    }

    private static void RunConsumer()
    {
        foreach (var item in Messages.GetConsumingEnumerable())
        {
            Cts.Token.ThrowIfCancellationRequested();
            Console.WriteLine($"Removed the {item} from the collection");
            Thread.Sleep(RandomGenerator.Next(1000));
        }
    }

    private static void RunProducer()
    {
        while (true)
        {
            Cts.Token.ThrowIfCancellationRequested();
            var i = RandomGenerator.Next(100);
            Messages.Add(i);
            Console.WriteLine($"Adding {i} to the collection");
            Thread.Sleep(RandomGenerator.Next(100));
        }
    }
}