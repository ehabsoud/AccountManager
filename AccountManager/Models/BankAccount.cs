namespace AccountManager.Models;
/// <summary>
/// Represents a bank account with balance, account details and transaction history.
/// </summary>
public class BankAccount
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public CurrencyType CurrencyType { get; set; }
    public AccountType AccountType { get; set; }
    public DateTime LastUpdate { get; set; }
    public List<Transaction> TransactionHistory { get; set; }

    /// <summary>
    /// Initializes a new instance of BankAccount with an empty transaction history.
    /// </summary>
    public BankAccount()
    {
        TransactionHistory = new List<Transaction>();
        LastUpdate = DateTime.Now;
    }

}