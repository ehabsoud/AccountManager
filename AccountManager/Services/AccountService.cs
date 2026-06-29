using AccountManager.Models;

namespace AccountManager.Services;

public class AccountService : IAccountService
{
    private List<BankAccount> _accounts = new List<BankAccount>();

    private bool _initialized = false;
    
    
    private readonly IStorageService _storageService;
    
    public AccountService(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;
        _initialized = true;
        _accounts = await _storageService.LoadAccounts();
    }
    
    public async Task CreateAccount(BankAccount account)
    {
        _accounts.Add(account);
        await _storageService.SaveAccounts(_accounts);
        
        Console.WriteLine($"Account created: {account.Name} ({account.Id})");
    }

    public async Task Deposit(Guid accountId, decimal amount)
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
        await _storageService.SaveAccounts(_accounts);

        Console.WriteLine($"Deposited {amount} into {account.Name} ({account.Id}). New balance: {account.Balance} SEK");
    }

    public async Task Withdraw(Guid accountId, decimal amount)
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
        await _storageService.SaveAccounts(_accounts);
        
        Console.WriteLine($"Withdrew {amount} from {account.Name} ({account.Id}). New balance: {account.Balance} SEK");
    }

    public async Task Transfer(Guid fromAccountId, Guid toAccountId, decimal amount)
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
        await _storageService.SaveAccounts(_accounts);
        
        Console.WriteLine($"Transferred {amount} SEK from {fromAccount.Name} to {toAccount.Name}. " +
                          $"New balances: {fromAccount.Balance} SEK / {toAccount.Balance} SEK");
    }

    public Task<List<Transaction>> GetTransactionHistory(Guid accountId)
    {
        var account = _accounts.FirstOrDefault(a => a.Id == accountId);

        if (account == null)
        {
            throw new Exception("Account not found");
        }
        return Task.FromResult(account.TransactionHistory);
    }

    public Task<List<BankAccount>> GetAccounts()
    {
        return Task.FromResult(_accounts);
    }
}