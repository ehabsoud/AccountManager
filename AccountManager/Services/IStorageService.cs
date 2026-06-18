using AccountManager.Models;

namespace AccountManager.Services;

/// <summary>
/// Defines a contract for saving and loading bank accounts.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Saves the provided list of bank accounts to storage.
    /// </summary>
    /// <param name="accounts">The list of bank accounts to save.</param>
    Task SaveAccounts(List<BankAccount> accounts);
    
    /// <summary>
    /// Loads and returns all saved bank accounts.
    /// </summary>
    /// <returns>A list of saved bank accounts.</returns>
    Task<List<BankAccount>> LoadAccounts();
}