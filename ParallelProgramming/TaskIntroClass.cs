namespace ParallelProgramming;

public class TaskIntroClass
{
    private static void Write(char c)
    {
        var i = 1000;
        while (i-- > 0)
        {
            Console.Write(i);
        }
    }

    private static void Write(object o)
    {
        var i = 1000;
        while (i-- > 0)
        {
            Console.Write(o);
        }
    }

    public void TaskBuilderMethod()
    {
        // Using Task.Factory
        Task.Factory.StartNew(() => Write('.'));
        
        // By instantiated the Task class
        var task = new Task(() => Write('.'));
        task.Start();
        
        // Using StartNew you declare a task and you start it 
        
        // Another overloaded alternative to run a task
        var t = new Task(Write, "hello");
        t.Start();
    }
    
}