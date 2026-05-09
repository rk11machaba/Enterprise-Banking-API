using EnterpriseBankingAPI.Application.Common;
using EnterpriseBankingAPI.Application.DTOs.Banking;
using EnterpriseBankingAPI.Application.Interfaces.Banking;
using EnterpriseBankingAPI.Domain.Entities;
using EnterpriseBankingAPI.Domain.Enums;
using EnterpriseBankingAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseBankingAPI.Infrastructure.Services.Banking;

public class BankAccountService : IBankAccountService
{
    private readonly ApplicationDbContext _context;

    public BankAccountService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResult<BankAccountDto>> CreateAccountAsync(Guid userId, CreateAccountRequestDto request)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return ServiceResult<BankAccountDto>.Fail("User not found.");

        var accountNumber = GenerateAccountNumber();

        var account = new BankAccount
        {
            AccountNumber = accountNumber,
            Currency = request.Currency,
            Balance = 0,
            IsActive = true,
            UserId = userId
        };

        _context.BankAccounts.Add(account);
        await _context.SaveChangesAsync();

        return ServiceResult<BankAccountDto>.Ok(MapToDto(account));
    }

    public async Task<ServiceResult<List<BankAccountDto>>> GetUserAccountsAsync(Guid userId)
    {
        var accounts = await _context.BankAccounts
            .Where(a => a.UserId == userId)
            .Select(a => MapToDto(a))
            .ToListAsync();

        return ServiceResult<List<BankAccountDto>>.Ok(accounts);
    }

    public async Task<ServiceResult<BankAccountDto>> GetAccountByIdAsync(Guid userId, Guid accountId)
    {
        var account = await _context.BankAccounts
            .FirstOrDefaultAsync(a => a.Id == accountId && a.UserId == userId);

        if (account == null)
            return ServiceResult<BankAccountDto>.Fail("Account not found.");

        return ServiceResult<BankAccountDto>.Ok(MapToDto(account));
    }

    public async Task<ServiceResult<TransactionDto>> DepositAsync(Guid userId, DepositRequestDto request)
    {
        if (request.Amount <= 0)
            return ServiceResult<TransactionDto>.Fail("Amount must be greater than zero.");

        var account = await _context.BankAccounts
            .FirstOrDefaultAsync(a => a.Id == request.AccountId && a.UserId == userId);

        if (account == null)
            return ServiceResult<TransactionDto>.Fail("Account not found.");

        if (!account.IsActive)
            return ServiceResult<TransactionDto>.Fail("Account is not active.");

        account.Balance += request.Amount;
        account.UpdatedAt = DateTime.UtcNow;

        var transaction = new Transaction
        {
            FromAccountId = account.Id,
            ToAccountId = account.Id,
            Amount = request.Amount,
            Reference = request.Reference,
            TransactionType = TransactionType.Deposit
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return ServiceResult<TransactionDto>.Ok(MapToDto(transaction));
    }

    public async Task<ServiceResult<TransactionDto>> WithdrawAsync(Guid userId, WithdrawRequestDto request)
    {
        if (request.Amount <= 0)
            return ServiceResult<TransactionDto>.Fail("Amount must be greater than zero.");

        var account = await _context.BankAccounts
            .FirstOrDefaultAsync(a => a.Id == request.AccountId && a.UserId == userId);

        if (account == null)
            return ServiceResult<TransactionDto>.Fail("Account not found.");

        if (!account.IsActive)
            return ServiceResult<TransactionDto>.Fail("Account is not active.");

        if (account.Balance < request.Amount)
            return ServiceResult<TransactionDto>.Fail("Insufficient funds.");

        account.Balance -= request.Amount;
        account.UpdatedAt = DateTime.UtcNow;

        var transaction = new Transaction
        {
            FromAccountId = account.Id,
            ToAccountId = account.Id,
            Amount = request.Amount,
            Reference = request.Reference,
            TransactionType = TransactionType.Withdrawal
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return ServiceResult<TransactionDto>.Ok(MapToDto(transaction));
    }

    public async Task<ServiceResult<TransactionDto>> TransferAsync(Guid userId, TransferRequestDto request)
    {
        if (request.Amount <= 0)
            return ServiceResult<TransactionDto>.Fail("Amount must be greater than zero.");

        if (request.FromAccountId == request.ToAccountId)
            return ServiceResult<TransactionDto>.Fail("Cannot transfer to the same account.");

        await using var dbTransaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var fromAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(a => a.Id == request.FromAccountId && a.UserId == userId);

            if (fromAccount == null)
                return ServiceResult<TransactionDto>.Fail("Source account not found.");

            if (!fromAccount.IsActive)
                return ServiceResult<TransactionDto>.Fail("Source account is not active.");

            if (fromAccount.Balance < request.Amount)
                return ServiceResult<TransactionDto>.Fail("Insufficient funds.");

            var toAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(a => a.Id == request.ToAccountId);

            if (toAccount == null)
                return ServiceResult<TransactionDto>.Fail("Destination account not found.");

            if (!toAccount.IsActive)
                return ServiceResult<TransactionDto>.Fail("Destination account is not active.");

            fromAccount.Balance -= request.Amount;
            fromAccount.UpdatedAt = DateTime.UtcNow;

            toAccount.Balance += request.Amount;
            toAccount.UpdatedAt = DateTime.UtcNow;

            var transaction = new Transaction
            {
                FromAccountId = request.FromAccountId,
                ToAccountId = request.ToAccountId,
                Amount = request.Amount,
                Reference = request.Reference,
                TransactionType = TransactionType.Transfer
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            await dbTransaction.CommitAsync();

            return ServiceResult<TransactionDto>.Ok(MapToDto(transaction));
        }
        catch
        {
            await dbTransaction.RollbackAsync();
            throw;
        }
    }

    private static string GenerateAccountNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd");
        var random = Random.Shared.Next(100000, 999999);
        return $"ACC{timestamp}{random}";
    }

    private static BankAccountDto MapToDto(BankAccount account) => new()
    {
        Id = account.Id,
        AccountNumber = account.AccountNumber,
        Balance = account.Balance,
        Currency = account.Currency,
        IsActive = account.IsActive,
        CreatedAt = account.CreatedAt
    };

    private static TransactionDto MapToDto(Transaction transaction) => new()
    {
        Id = transaction.Id,
        Type = transaction.TransactionType.ToString(),
        Amount = transaction.Amount,
        Reference = transaction.Reference,
        FromAccountId = transaction.FromAccountId,
        ToAccountId = transaction.ToAccountId,
        CreatedAt = transaction.CreatedAt
    };
}
