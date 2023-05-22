namespace ParallelProgramming;

public class FactoryPatternExample
{
    private FactoryPatternExample() {}

    private async Task<FactoryPatternExample> InitAsync()
    {
        await Task.Delay(1000);
        return this;
    }

    public static Task<FactoryPatternExample> CreateAsync()
    {
        var result = new FactoryPatternExample();
        return result.InitAsync();
    }
    
    // This will be called: FactoryPatternExample fps = async FactoryPatternExample.CreateAsync();
}