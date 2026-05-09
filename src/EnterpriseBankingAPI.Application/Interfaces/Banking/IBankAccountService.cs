using EnterpriseBankingAPI.Application.Common;
using EnterpriseBankingAPI.Application.DTOs.Banking;

namespace EnterpriseBankingAPI.Application.Interfaces.Banking;

public interface IBankAccountService
{
    Task<ServiceResult<BankAccountDto>> CreateAccountAsync(Guid userId, CreateAccountRequestDto request);
    Task<ServiceResult<List<BankAccountDto>>> GetUserAccountsAsync(Guid userId);
    Task<ServiceResult<BankAccountDto>> GetAccountByIdAsync(Guid userId, Guid accountId);
    Task<ServiceResult<TransactionDto>> DepositAsync(Guid userId, DepositRequestDto request);
    Task<ServiceResult<TransactionDto>> WithdrawAsync(Guid userId, WithdrawRequestDto request);
    Task<ServiceResult<TransactionDto>> TransferAsync(Guid userId, TransferRequestDto request);
}
