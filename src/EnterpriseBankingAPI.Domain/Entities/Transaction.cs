using EnterpriseBankingAPI.Domain.Common;

namespace EnterpriseBankingAPI.Domain.Entities;

public class Transaction : BaseEntity
{
    public Guid FromAccountId { get; set; }
    
    public BankAccount FromAccount { get; set; } = null;

    public Guid ToAccountId { get; set; }

    public BankAccount ToAccount { get; set; } = null;

    public decimal Amount { get; set; }

    public string Reference { get; set; } = string.Empty;

    public string TransactionType { get; set; } = string.Empty;
    
}