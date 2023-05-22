namespace ParallelProgramming;

public class TaskCoordinationExample
{

    public void ParentChildTasks()
    {
        var parent = new Task(() =>
        {
            // detached task
            var child = new Task(() =>
            {
                Console.WriteLine("Child task started");
                Thread.Sleep(3000);
                Console.WriteLine("Child task finished");
                // throw new Exception();
            }, TaskCreationOptions.AttachedToParent);
            
            var completionHandler = child.ContinueWith(t =>
            {
               Console.WriteLine($"Hooray, task {t.Id}'s state is {t.Status}"); 
            }, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnRanToCompletion);

            var failedHandler = child.ContinueWith(t =>
            {
                Console.WriteLine($"Ops, task {t.Id}'s state is {t.Status}");
            }, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnFaulted);
            
            child.Start();

        });
        
        parent.Start();

        try
        {
            parent.Wait();
        }
        catch (AggregateException ae)
        {
            ae.Handle(e => true);
        }
    }
    
    public void MultiTaskContinuation()
    {
        var task1 = Task.Factory.StartNew(() => "Task 1");
        var task2 = Task.Factory.StartNew(() => "Task 2");

        // An alternative is to use ContinueWhenAny
        var task3 = Task.Factory.ContinueWhenAll(new[] { task1, task2 },
            tasks =>
            {
                Console.WriteLine("Tasks has been completed:");
                foreach (var t in tasks) 
                {
                    Console.WriteLine(" - " + t.Result);
                }
                Console.WriteLine("All tasks completed");
            });
        task3.Wait();
    }
    
    public void TaskContinuation()
    {
        var task1 = Task.Factory.StartNew(() =>
        {
            Console.WriteLine("Boiling water");
        });

        var task2 = task1.ContinueWith(t =>
        {
            Console.WriteLine($"Completed task {t.Id}, pour water into cup.");
        });

        task2.Wait();
    }
}