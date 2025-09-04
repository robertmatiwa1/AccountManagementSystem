using AccountManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class TempController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public TempController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("accounts-count")]
    public async Task<IActionResult> GetAccountsCount()
    {
        try
        {
            var count = await _db.Accounts.CountAsync();
            return Ok(new { AccountsCount = count });
        }
        catch (Exception ex)
        {
            return Problem($"Error connecting to database: {ex.Message}");
        }
    }
}
