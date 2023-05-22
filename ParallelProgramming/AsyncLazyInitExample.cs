namespace ParallelProgramming;

public class AsyncLazyInitExample
{
    private static int value;

    private readonly Lazy<Task<int>> AutoIncValue = new Lazy<Task<int>>(async () =>
    {
        await Task.Delay(1000).ConfigureAwait(false);
        return value++;
    });

    private readonly Lazy<Task<int>> AutoIncValue2 =
        new Lazy<Task<int>>(() => Task.Run(() =>
        {
            Task.Delay(1000).ConfigureAwait(false);
            return value++;
        }));
    


    public async Task UseValue()
    {
        var value = await AutoIncValue.Value;
    }
}