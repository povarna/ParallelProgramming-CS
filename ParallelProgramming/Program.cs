// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;

namespace ParallelProgramming;

public class Program
{

    public static void Main(string[] args)
    {
        Console.WriteLine("Main program starts!");
        // Task.Factory.StartNew(AddParis).Wait();
        // AddParis();
        // CollectionOperations();
        // QueueOperations();
        // BagOperations();
        
        
        // Producer Consumer Demo
        // Task.Factory.StartNew(ProduceAndConsume, Cts.Token);
        // Console.ReadKey();
        // Cts.Cancel();
        
        // Task Coordination
        // var taskCoordinationExample = new TaskCoordinationExample();
        // taskCoordinationExample.ParentChildTasks();

        // Barrier
        // var barrierExample = new BarrierExample();
        // barrierExample.Run();

        // CountDown Event
        // var countDownEvenExample = new CountDownEvenExample();
        // countDownEvenExample.Run();
        
        // Semaphore slim example
        // var semaphoreSlimExample = new SemaphoreSlimExample();
        // semaphoreSlimExample.Run();

        // Parallel Collections
        // var parallelLoopsExamples = new ParallelLoopsExamples();
        // parallelLoopsExamples.DemoFunction();
        //
        
        // ThreadLocal Example
        // var threadLocalExample = new ThreadLocalExample();
        // threadLocalExample.Run();
        
        
        // Partitioning
        var summary = BenchmarkRunner.Run<Program>();
        Console.WriteLine(summary);
        Console.ReadKey();
        
    }
    
}