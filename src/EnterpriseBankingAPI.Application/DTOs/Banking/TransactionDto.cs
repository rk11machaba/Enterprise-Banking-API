namespace EnterpriseBankingAPI.Application.DTOs.Banking;

public class TransactionDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Reference { get; set; } = string.Empty;
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public DateTime CreatedAt { get; set; }
}
