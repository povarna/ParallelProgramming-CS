using System.Collections.Concurrent;

namespace ParallelProgramming;

public static class ParallelCollectionsExamples
{
    private static readonly ConcurrentDictionary<string, string> Capitals = new();
    private static readonly ConcurrentQueue<int> Queue = new();
    private static readonly ConcurrentStack<int> Stack = new();
    
    // There is no concurrent list in tpl
    // stack LIFO
    // queue FIFO
    // no ordering => bag
    private static readonly ConcurrentBag<int> Bag = new();

    public static void AddParis()
    {
        var success = Capitals.TryAdd("France", "Paris");
        var who = Task.CurrentId.HasValue ? ("Task " + Task.CurrentId) : "Main Thread";
        Console.WriteLine($"{who} {(success ? "added" : "did not add")} the element");
    }

    public static void CollectionOperations()
    {
        // You can use the square brackets
        Capitals["Russia"] = "Leningrad";

        // update with canonical implementation
        Capitals.AddOrUpdate("Russia", "Moscow", (k, old) => old + " --> Moscow");
        Console.WriteLine($"The capital of Russia is: {Capitals["Russia"]}");

        Capitals["Sweden"] = "Uppsala";
        var capitalOfSweden = Capitals.GetOrAdd("Sweden", "Stockholm");
        Console.WriteLine($"The capital of Sweeden is: {capitalOfSweden}");

        const string toRemove = "Russia";

        var didRemove = Capitals.TryRemove(toRemove, out var removed);
        if (didRemove)
        {
            Console.WriteLine($"We just removed: {removed}");
        }
        else
        {
            Console.WriteLine($"We failed to remove the capital of {toRemove}");
        }

        // Expensive operation
        var count = Capitals.Count;

        // Enumerate the collection
        foreach (var capital in Capitals)
        {
            Console.WriteLine($"The capital of {capital.Key} is {capital.Value}");
        }
    }

    public static void QueueOperations()
    {
        Queue.Enqueue(1);
        Queue.Enqueue(2);

        if (Queue.TryDequeue(out var result))
        {
            Console.WriteLine($"Remove element {result}");
        }

        if (Queue.TryPeek(out result))
        {
            Console.WriteLine($"Front of the element is {result}");
        }
    }

    public static void StackOperations()
    {
        Stack.Push(1);
        Stack.Push(2);
        Stack.Push(3);
        Stack.Push(4);

        if (Stack.TryPeek(out var peekResult))
            Console.WriteLine($"{peekResult} is on top");

        if (Stack.TryPop(out var popResult))
            Console.WriteLine($"Popped {popResult}");

        // initialize an array of 5 elements with 0 
        var items = new int[5];
        if (Stack.TryPopRange(items, 0, 5) > 0)
        {
            var text = string.Join(", ", items.Select(i => i.ToString()));
            Console.WriteLine($"Popped these items: {text}");
        }
    }

    public static void BagOperations()
    {
        var bag = new ConcurrentBag<int>();
        var tasks = new List<Task>();
        
        for (var i = 0; i < 10; i++)
        {
            var i1 = i;
            tasks.Add(Task.Factory.StartNew(() =>
            {
                bag.Add(i1);
                Console.WriteLine($"Task - {Task.CurrentId} has added {i1}");
                if (bag.TryPeek(out var result))
                {
                    Console.WriteLine($"Task - {Task.CurrentId} has peeked the value: {result}");
                }
            }));

            Task.WaitAll(tasks.ToArray());

            if (bag.TryTake(out var last))
            {
                Console.WriteLine($"I got {last}");
            }

        }
    }
}