using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountManagementSystem.Data; // Make sure this matches your DbContext namespace
using AccountManagementSystem.Models; // If needed for your tables

namespace AccountManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public TestController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("test-db")]
        public async Task<IActionResult> TestDb()
        {
            try
            {
                var count = await _db.Accounts.CountAsync(); // Replace 'Accounts' with your table name
                return Ok($"Database connected! Accounts count: {count}");
            }
            catch (Exception ex)
            {
                return Problem($"Error connecting to database: {ex.Message}");
            }
        }
    }
}
