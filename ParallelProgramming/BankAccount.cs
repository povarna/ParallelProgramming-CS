namespace ParallelProgramming;

public class BankAccount
{
    public object padlock = new object();
    private int _balance;

    public int Balance
    {
        get => _balance;
        private set => _balance = value;
    }

    public void Deposit(int amount)
    {
        _balance += amount;
    }

    public void Withdraw(int amount)
    {
        _balance -= amount;
    }

    public void DepositWithLock(int amount)
    {
        lock (padlock)
        {
            Balance += amount;
        }
    }
    
    public void DepositWithInterlocked(int amount)
    {
        Interlocked.Add(ref _balance, amount);
    }
    
    public void WithdrawWithLock(int amount)
    {
        lock (padlock)
        {
            Balance -= amount;
        }
    }
    
    public void WithdrawWithInterlocked(int amount)
    {
        Interlocked.Add(ref _balance, -amount);
    }
}