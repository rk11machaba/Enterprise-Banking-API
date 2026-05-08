using EnterpriseBankingAPI.Domain.Common;

namespace EnterpriseBankingAPI.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set;} = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set;} = "Customer";

    public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    
}