namespace ParallelProgramming;

public static class SynchronizationConcepts
{
    
    private static void UpgradableReadLock()
    {
        // Write values after a read lock has been acquired

        var x = 0;
        var padLock = new ReaderWriterLockSlim();

        var tasks = new List<Task>();
        for (var i = 0; i < 10; i++)
        {
            tasks.Add(Task.Factory.StartNew(() =>
            {
                padLock.EnterUpgradeableReadLock();
                Console.WriteLine($"Entered Read Lock");
                if (i % 2 == 0)
                {
                    padLock.EnterWriteLock();
                    Console.WriteLine($"Entered Write Lock. Updating the value");
                    x = 123;
                    padLock.ExitWriteLock();
                    Console.WriteLine($"Exited write lock, x = {x}");
                }

                Thread.Sleep(5000);
                padLock.ExitUpgradeableReadLock();
                Console.WriteLine($"Exited read lock");
            }));
        }

        Task.WaitAll(tasks.ToArray());
    }

    private static void ReaderLockDemo()
    {
        // Works with recursion => when you lock a resource twice. 
        var padLock = new ReaderWriterLockSlim();
        var x = 0;
        var random = new Random();

        var tasks = new List<Task>();
        for (var i = 0; i < 10; i++)
        {
            tasks.Add(Task.Factory.StartNew(() =>
            {
                padLock.EnterReadLock();
                Console.WriteLine($"Entered read lock, x = {x}");
                Thread.Sleep(5000);
                padLock.ExitReadLock();
                Console.WriteLine($"Exited read lock, x = {x}");
            }));
        }

        try
        {
            Task.WaitAll(tasks.ToArray());
        }
        catch (AggregateException aggregateException)
        {
            aggregateException.Handle(e =>
            {
                Console.WriteLine(e);
                return true;
            });
        }

        // Input loop where I'm changing the value of x
        while (true)
        {
            Console.ReadKey();
            padLock.EnterWriteLock();
            Console.Write("Write lock acquired");
            var newValue = random.Next(10);
            x = newValue;
            Console.WriteLine($"Set x = {x}");
            padLock.ExitWriteLock();
            Console.WriteLine("Write lock released!");
        }
    }

    private static void BankAccountWithMutex()
    {
        var tasks = new List<Task>();
        var ba = new BankAccount();

        var mutex = new Mutex();

        for (var i = 0; i < 10; i++)
        {
            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (var j = 0; j < 1000; j++)
                {
                    var haveLock = mutex.WaitOne();
                    try
                    {
                        ba.Deposit(100);
                    }
                    finally
                    {
                        if (haveLock) mutex.ReleaseMutex();
                    }
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (var j = 0; j < 1000; j++)
                {
                    var haveLock = mutex.WaitOne();

                    try
                    {
                        ba.Withdraw(100);
                    }
                    finally
                    {
                        if (haveLock) mutex.ReleaseMutex();
                    }
                }
            }));
        }

        Task.WaitAll(tasks.ToArray());

        Console.WriteLine($"Final balance is {ba.Balance}");
    }

    private static void BankAccountDemoWithSpinLock()
    {
        var tasks = new List<Task>();
        var ba = new BankAccount();

        SpinLock sl = new SpinLock();

        for (var i = 0; i < 10; i++)
        {
            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (int j = 0; j < 1000; j++)
                {
                    var lockTaken = false;
                    try
                    {
                        sl.Enter(ref lockTaken);
                        ba.Deposit(1000);
                    }
                    finally
                    {
                        if (lockTaken) sl.Exit();
                    }
                }
            }));
        }

        for (var i = 0; i < 10; i++)
        {
            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (int j = 0; j < 1000; j++)
                {
                    var lockTaken = false;
                    try
                    {
                        sl.Enter(ref lockTaken);
                        ba.Withdraw(1000);
                    }
                    finally
                    {
                        if (lockTaken) sl.Exit();
                    }
                }
            }));
        }

        Task.WaitAll(tasks.ToArray());

        Console.WriteLine($"Final balance is {ba.Balance}");
    }

    private static void BankAccountDemo()
    {
        var tasks = new List<Task>();
        var ba = new BankAccount();

        for (var i = 0; i < 10; i++)
        {
            tasks.Add(Task.Factory.StartNew(() => { ba.DepositWithLock(1000); }));
        }

        for (var i = 0; i < 10; i++)
        {
            tasks.Add(Task.Factory.StartNew(() => { ba.WithdrawWithLock(1000); }));
        }

        Task.WaitAll(tasks.ToArray());

        Console.WriteLine($"Final balance is {ba.Balance}");
    }

    private static void ExceptionTaskHandller()
    {
        var t1 = InvalidOperationTask();
        var t2 = AccessViolationTask();

        try
        {
            Task.WaitAll(t1, t2);
        }
        catch (AggregateException ex)
        {
            foreach (var e in ex.InnerExceptions)
            {
                Console.WriteLine($"Exception {e.GetType()} from {e.Source}");
            }
        }

        Console.ReadKey();
    }

    private static Task InvalidOperationTask()
    {
        return Task.Factory.StartNew(() => throw new InvalidOperationException("Can't do this") { Source = "t1" });
    }

    private static Task AccessViolationTask()
    {
        return Task.Factory.StartNew(() => throw new AccessViolationException("Can't access this") { Source = "t2" });
    }

    private static void WaitTaskMethod()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        var t1 = new Task(() =>
        {
            for (var i = 0; i < 5; i++)
            {
                token.ThrowIfCancellationRequested();
                Thread.Sleep(1000);
            }

            Console.WriteLine("I take 5 seconds");
        }, token);
        t1.Start();

        var t2 = Task.Factory.StartNew(() => Thread.Sleep(3000), token);

        // Waiting
        // t1.Wait(token);
        // // We can wait for both tasks
        // Task.WaitAll(t1, t2);
        //
        // // We can wait for only one task: WaitAny / Wait
        // Task.WaitAny(t1, t2);
        //
        // You can add a timeout in ms, but the task will need to be added as an array
        Task.WaitAny(new[] { t1, t2 }, 4000, token);
        // Get the status of the tasks:
        Console.WriteLine($"Task t status is {t1.Status}");
        Console.WriteLine($"Task t status is {t2.Status}");
    }

    private static void SleepThreads()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        var t = new Task(() =>
        {
            Console.WriteLine("Press any key to disarm; you have 5 seconds");
            var cancelled = token.WaitHandle.WaitOne(5000);
            Console.WriteLine(cancelled ? "Bob disarmed. " : "boom!");
        }, token);
        t.Start();
    }

    private static void CancellationExamples()
    {
        var planned = new CancellationTokenSource();
        var preventative = new CancellationTokenSource();
        var emergency = new CancellationTokenSource();

        var paranoid = CancellationTokenSource.CreateLinkedTokenSource(
            planned.Token, preventative.Token, emergency.Token
        );

        Task.Factory.StartNew(() =>
        {
            var i = 0;
            while (true)
            {
                paranoid.Token.ThrowIfCancellationRequested();
                Console.WriteLine($"{i++}\t");
                Thread.Sleep(1000);
            }
        }, paranoid.Token);

        Console.ReadKey();
        Console.ReadKey();
        emergency.Cancel();
    }

    private static void InfiniteTask()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        token.Register(() =>
            Console.WriteLine("Cancellation has been requested"));

        var t = new Task(() =>
        {
            var i = 0;
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }
                // equivalent is: token.ThrowIfCancellationRequested();

                Console.WriteLine($"{i++}\t");
            }
        }, token);
        t.Start();

        Task.Factory.StartNew(() =>
            {
                token.WaitHandle.WaitOne();
                Console.WriteLine("The wait handle released, cancellation was request");
            }
        );

        Console.ReadKey();
        cts.Cancel();
    }

    private static void CreatingAndRunningTasksExample()
    {
        var text1 = "testing";
        var text2 = "this";

        var task1 = new Task<int>(TextLength, text1);
        task1.Start();
        // can be written using TaskFactory
        var task2 = Task.Factory.StartNew(TextLength, text2);

        Console.WriteLine("Getting the result!");
        Console.WriteLine($"Length of '{text1}' is {task1.Result} ");
        Console.WriteLine($"Length of '{text2}' is {task2.Result} ");
        Console.WriteLine("Main program done.");
    }

    private static int TextLength(object o)
    {
        Console.WriteLine($"Task with id {Task.CurrentId} processing object {o} ...");
        return o.ToString().Length;
    }
}