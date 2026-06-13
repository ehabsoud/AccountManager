using AccountManager.Models;

namespace AccountManager.Services;

public class AccountService : IAccountService
{
    private List<BankAccount> _accounts = new List<BankAccount>();
    
    public void CreateAccount(BankAccount account)
    {
        _accounts.Add(account);
    }

    public void Deposit(Guid accountId, decimal amount)
    {
        var account = _accounts.FirstOrDefault(a => a.Id == accountId);
        
        if (account == null)
        {
            throw new Exception("Account not found");
        }
        decimal balanceBefore = account.Balance;
        account.Balance += amount;
        account.LastUpdate = DateTime.Now;
        
        var transaction = new Transaction
        {
            TransactionType = TransactionType.Deposit,
            Amount = amount,
            Date = DateTime.Now,
            BalanceBefore = balanceBefore,
            BalanceAfter = account.Balance,
        };
        
        account.TransactionHistory.Add(transaction);
    }

    public void Withdraw(Guid accountId, decimal amount)
    {
        var account = _accounts.FirstOrDefault(a => a.Id == accountId);

        if (account == null)
        {
            throw new Exception("Account not found");
        }
        
        decimal balanceBefore = account.Balance;
        if (account.Balance < amount)
        {
            throw new Exception("Insufficient balance");
        }
        account.Balance -= amount;
        account.LastUpdate = DateTime.Now;

        var transaction = new Transaction
        {
            TransactionType = TransactionType.Withdraw,
            Amount = amount,
            Date = DateTime.Now,
            BalanceBefore = balanceBefore,
            BalanceAfter = account.Balance,
        };
        
        account.TransactionHistory.Add(transaction);
    }

    public void Transfer(Guid fromAccountId, Guid toAccountId, decimal amount)
    {
        var fromAccount = _accounts.FirstOrDefault(a => a.Id == fromAccountId);

        if (fromAccount == null)
        {
            throw new Exception("Account not found");
        }
        
        var toAccount = _accounts.FirstOrDefault(a => a.Id == toAccountId);

        if (toAccount == null)
        {
            throw new Exception("Account not found");
        }
        
        decimal balanceBefore = fromAccount.Balance;
        if (fromAccount.Balance < amount)
        {
            throw new Exception("Insufficient balance");
        }
        fromAccount.Balance -= amount;
        decimal toBalanceBefore = toAccount.Balance;
        toAccount.Balance += amount;
        fromAccount.LastUpdate = DateTime.Now;
        toAccount.LastUpdate = DateTime.Now;

        var fromTransaction = new Transaction
        {
            TransactionType = TransactionType.Transfer,
            Amount = amount,
            Date = DateTime.Now,
            BalanceBefore = balanceBefore,
            BalanceAfter = fromAccount.Balance,
            FromAccount = fromAccountId,
            ToAccount = toAccountId,
        };
        
        fromAccount.TransactionHistory.Add(fromTransaction);

        var toTransaction = new Transaction
        {
            TransactionType = TransactionType.Transfer,
            Amount = amount,
            Date = DateTime.Now,
            BalanceBefore = toBalanceBefore,
            BalanceAfter = toAccount.Balance,
            FromAccount = fromAccountId,
            ToAccount = toAccountId,
        };
        
        toAccount.TransactionHistory.Add(toTransaction);
    }

    public List<Transaction> GetTransactionHistory(Guid accountId)
    {
        var account = _accounts.FirstOrDefault(a => a.Id == accountId);

        if (account == null)
        {
            throw new Exception("Account not found");
        }
        return account.TransactionHistory;
    }

    public List<BankAccount> GetAccounts()
    {
        return _accounts;
    }
}