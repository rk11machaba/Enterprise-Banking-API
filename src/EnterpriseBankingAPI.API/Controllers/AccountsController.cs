using System.Security.Claims;
using EnterpriseBankingAPI.Application.DTOs.Banking;
using EnterpriseBankingAPI.Application.Interfaces.Banking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseBankingAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly IBankAccountService _bankAccountService;

    public AccountsController(IBankAccountService bankAccountService)
    {
        _bankAccountService = bankAccountService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(CreateAccountRequestDto request)
    {
        var userId = GetUserId();
        var result = await _bankAccountService.CreateAccountAsync(userId, request);

        if (!result.Success)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetAccount), new { id = result.Data!.Id }, result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts()
    {
        var userId = GetUserId();
        var result = await _bankAccountService.GetUserAccountsAsync(userId);

        return Ok(result.Data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAccount(Guid id)
    {
        var userId = GetUserId();
        var result = await _bankAccountService.GetAccountByIdAsync(userId, id);

        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Data);
    }

    [HttpPost("{id:guid}/deposit")]
    public async Task<IActionResult> Deposit(Guid id, DepositRequestDto request)
    {
        request.AccountId = id;
        var userId = GetUserId();
        var result = await _bankAccountService.DepositAsync(userId, request);

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpPost("{id:guid}/withdraw")]
    public async Task<IActionResult> Withdraw(Guid id, WithdrawRequestDto request)
    {
        request.AccountId = id;
        var userId = GetUserId();
        var result = await _bankAccountService.WithdrawAsync(userId, request);

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(TransferRequestDto request)
    {
        var userId = GetUserId();
        var result = await _bankAccountService.TransferAsync(userId, request);

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userIdClaim!);
    }
}
