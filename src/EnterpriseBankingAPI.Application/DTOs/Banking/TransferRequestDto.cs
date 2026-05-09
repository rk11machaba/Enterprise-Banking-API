namespace EnterpriseBankingAPI.Application.DTOs.Banking;

public class TransferRequestDto
{
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Reference { get; set; } = string.Empty;
}
