namespace EnterpriseBankingAPI.Application.DTOs.Banking;

public class WithdrawRequestDto
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Reference { get; set; } = string.Empty;
}
