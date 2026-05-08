using EnterpriseBankingAPI.Domain.Common;

namespace EnterpriseBankingAPI.Domain.Entities;

public class AuditLog : BaseEntity
{
    public Guid UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    
    public string IPAddress { get; set; } = string.Empty;
    
}