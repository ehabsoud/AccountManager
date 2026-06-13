using AccountManager.Models;

namespace AccountManager.Services;

public interface IAccountService
{
    void CreateAccount(BankAccount account);
    void Deposit(Guid accountId, decimal amount);
    void Withdraw(Guid accountId, decimal amount);
    void Transfer(Guid fromAccountId, Guid toAccountId, decimal amount);
    List<Transaction> GetTransactionHistory(Guid accountId);
    List<BankAccount> GetAccounts();
}