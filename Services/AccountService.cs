using WeatherChecker_FO.Data;
using WeatherChecker_FO.Models;
using Microsoft.EntityFrameworkCore;

public interface IAccountService
{
    Task<bool> EmailExistsAsync(string email);
    Task RegisterUserAsync(string email, string password);
    Task<Account> GetByEmailAsync(string email);
    bool VerifyPassword(Account account, string password);
}

public class AccountService : IAccountService
{
    private readonly AppDbContext _db;

    public AccountService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _db.Accounts.AnyAsync(a => a.Email == email);
    }

    public async Task RegisterUserAsync(string email, string password)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var account = new Account
        {
            Email = email,
            PasswordHash = hashedPassword
        };

        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();
    }

    public async Task<Account> GetByEmailAsync(string email)
    {
        return await _db.Accounts.FirstOrDefaultAsync(a => a.Email == email);
    }

    public bool VerifyPassword(Account account, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, account.PasswordHash);
    }

}
