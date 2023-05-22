using System.Net;

namespace ParallelProgramming;

public class AsyncExamples
{
    // Long running task
    private async Task<int> CalculateValueAsync()
    {
        await Task.Delay(5000);
        return 123;
    }

    public async Task Run()
    {
        var calculateValue = await CalculateValueAsync();
        Console.Write($"Returned value is: {calculateValue}");
        
        await Task.Delay(5000);
        using (var wc = new WebClient())
        {
            var data = await wc.DownloadStringTaskAsync("https://goggle/com/robots.txt");
            var robotsResult = data.Split("\n")[0].Trim();
            Console.WriteLine(robotsResult);
        }
    }
}