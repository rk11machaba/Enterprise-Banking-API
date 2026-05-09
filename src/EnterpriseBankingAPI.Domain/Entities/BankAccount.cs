using EnterpriseBankingAPI.Domain.Common;
using EnterpriseBankingAPI.Domain.Entities;

namespace EnterpriseBankingAPI.Domain.Entities;

public class BankAccount : BaseEntity
{
    public string AccountNumber { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public string Currency { get; set; } = "ZAR";

    public bool IsActive { get; set; } = true;

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public ICollection<Transaction> SentTransactions { get; set; } = new List<Transaction>();

    public ICollection<Transaction> ReceivedTransactions { get; set; } = new List<Transaction>();

}