using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherChecker_FO.Services;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IJwtProviderService _jwtProviderService;

    public AuthController(IAccountService accountService, IJwtProviderService jwtProviderService)
    {
        _accountService = accountService;
        _jwtProviderService = jwtProviderService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (await _accountService.EmailExistsAsync(request.Email))
        {
            return BadRequest(new { message = "Użytkownik z takim emailem już istnieje." });
        }

        await _accountService.RegisterUserAsync(request.Email, request.Password);

        return Ok(new { message = "Zarejestrowano" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var account = await _accountService.GetByEmailAsync(request.Email);
        if (account == null || !_accountService.VerifyPassword(account, request.Password)) // tu zamień na hash w przyszłości
        {
            return Unauthorized(new { message = "Nieprawidłowy email lub hasło." });
        }

        var token = _jwtProviderService.GenerateToken(account.Email);
        return Ok(new { token });
    }
}

public record RegisterRequest(string Email, string Password);
public record LoginRequest(string Email, string Password);
