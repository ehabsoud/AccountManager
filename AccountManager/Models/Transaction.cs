namespace AccountManager.Models;

/// <summary>
/// Represents a transaction with type, including amount, balance changes and optional transfer details.
/// </summary>
public class Transaction
{
    public TransactionType TransactionType  { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public decimal BalanceBefore { get; set; }
    public decimal BalanceAfter { get; set; }
    public Guid? FromAccount  { get; set; }
    public Guid? ToAccount { get; set; }
}