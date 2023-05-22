namespace ParallelProgramming;

public class ParallelLoopsExamples
{

    public void DemoFunction()
    {
        
        // you can stop a loop using cancelation tokne
        var cts = new CancellationTokenSource();
        var po = new ParallelOptions();
        po.CancellationToken = cts.Token;
        
       var result = Parallel.For(0, 20, po, (int x, ParallelLoopState state) =>
        {
            Console.WriteLine($"{x}[Task-{Task.CurrentId}]\t");
            if (x == 10)
            {
                // This stop the execution of the loop as soon of possible
                // state.Stop();
                // Break will make a request to stop the iterations beyond this one. It's lees immediate stop 
                state.Break();
                // This will propagate to main so we need to catch it.
                // throw new Exception();
            }
        });
       
       Console.WriteLine();
       Console.WriteLine($"Was the loop completed: {result.IsCompleted}");
       if (result.LowestBreakIteration.HasValue)
       {
           Console.WriteLine($"Lowest break iteration is: {result.LowestBreakIteration}");
       }
    }
    
    public void Run()
    {
        var a = new Action(() => Console.WriteLine($"First {Task.CurrentId}"));
        var b = new Action(() => Console.WriteLine($"Second {Task.CurrentId}"));
        var c = new Action(() => Console.WriteLine($"Third {Task.CurrentId}"));
        
        Parallel.Invoke(a,b,c);

        Parallel.For(1, 11, i =>
        {
            Console.WriteLine($"{i * i} \t");
        });

        var words = new string[] { "oh", "what", "a", "night" };
        Parallel.ForEach(words, word =>
        {
            Console.WriteLine($"The {word} has length: {word.Length} (task {Task.CurrentId})");
        });
        
        // Running an iteration using a custom enumerator
        Parallel.ForEach(Range(1, 20, 3), Console.WriteLine);
    }

    private static IEnumerable<int> Range(int startValue, int endValue, int step)
    {
        for (var i = startValue; i < endValue; i += step)
        {
            yield return i;
        }
    }
}