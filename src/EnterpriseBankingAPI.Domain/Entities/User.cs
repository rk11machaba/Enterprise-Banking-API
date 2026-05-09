using EnterpriseBankingAPI.Domain.Common;
using EnterpriseBankingAPI.Domain.Enums;

namespace EnterpriseBankingAPI.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set;} = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Customer;

    public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    
}