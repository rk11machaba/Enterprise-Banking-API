using EnterpriseBankingAPI.Domain.Entities;

namespace EnterpriseBankingAPI.Application.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}