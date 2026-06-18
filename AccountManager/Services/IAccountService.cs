using AccountManager.Models;

namespace AccountManager.Services;

/// <summary>
/// Defines the contract for bank account management operations.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Initializes the service by loading saved accounts from local storage.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InitializeAsync();
    
    /// <summary>
    /// Creates a new bank account.
    /// </summary>
    /// <param name="account">The bank account to be added.</param>
    Task CreateAccount(BankAccount account);
    
    /// <summary>
    /// Deposits an amount into the specified account.
    /// </summary>
    /// <param name="accountId">The unique identifier of the account.</param>
    /// <param name="amount">The amount to be deposited.</param>
    Task Deposit(Guid accountId, decimal amount);
    
    /// <summary>
    /// Withdraws an amount from the specified account.
    /// </summary>
    /// <param name="accountId">The unique identifier of the account.</param>
    /// <param name="amount">The amount to withdraw.</param>
    Task Withdraw(Guid accountId, decimal amount);
    
    /// <summary>
    /// Transfer an amount between two accounts.
    /// </summary>
    /// <param name="fromAccountId">The account to transfer from.</param>
    /// <param name="toAccountId">The account to transfer to.</param>
    /// <param name="amount">The amount to transfer.</param>
    Task Transfer(Guid fromAccountId, Guid toAccountId, decimal amount);
    
    /// <summary>
    /// Retrieves the transaction history for the specified account.
    /// </summary>
    /// <param name="accountId">The unique identifier of the account.</param>
    /// <returns>A list of transactions for the specified account.</returns>
    Task<List<Transaction>> GetTransactionHistory(Guid accountId);
    
    /// <summary>
    /// Retrieves all bank accounts.
    /// </summary>
    /// <returns>A list of all bank accounts.</returns>
    Task<List<BankAccount>> GetAccounts();
}