namespace ParallelProgramming;

public class CancellationExample
{
    public void RunWithCancellationToken()
    {
        var items = ParallelEnumerable.Range(1, 20);
        var cts = new CancellationTokenSource();

        var results = items.WithCancellation(cts.Token).Select(i =>
        {
            var result = Math.Log10(i);
            Console.WriteLine($"i = {i}, taskId = {Task.CurrentId}");
            return result;
        });

        try
        {
            foreach (var result in results)
            {
                if (result > 1)
                    cts.Cancel();

                Console.WriteLine($"result = {result}");
            }
        }
        catch (AggregateException ae)
        {
            ae.Handle(ex =>
            {
                Console.WriteLine($"{ae.GetType().Name}: {ae.Message}");
                return true;
            });
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Canceled!!!");
        }
    }

    public void RunWithTryCatch()
    {
        // ParallelEnumerable returns a parallel query. It uses tasks for abstraction.
        var items = ParallelEnumerable.Range(1, 20);
        var results = items.Select(i =>
        {
            var result = Math.Log10(i);
            if (result > 1) throw new InvalidOperationException();
            Console.WriteLine($"i = {i}, taskId = {Task.CurrentId}");
            return result;
        });

        try
        {
            foreach (var result in results)
            {
                Console.WriteLine($"result = {result}");
            }
        }
        catch (AggregateException ae)
        {
            ae.Handle(ex =>
            {
                Console.WriteLine($"{ae.GetType().Name}: {ae.Message}");
                return true;
            });
        }
    }
}