using AccountManagementSystem.Data;
using AccountManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace AccountManagementSystem.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["AccountSortParm"] = String.IsNullOrEmpty(sortOrder) ? "account_desc" : "";
            ViewData["BalanceSortParm"] = sortOrder == "Balance" ? "balance_desc" : "Balance";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var accounts = _context.Accounts.Include(a => a.person_codeNavigation).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                accounts = accounts.Where(a =>
                    (a.account_number != null && a.account_number.Contains(searchString)) ||
                    (a.person_codeNavigation != null && a.person_codeNavigation.surname != null && 
                     a.person_codeNavigation.surname.Contains(searchString)));
            }

            switch (sortOrder)
            {
                case "account_desc":
                    accounts = accounts.OrderByDescending(a => a.account_number);
                    break;
                case "Balance":
                    accounts = accounts.OrderBy(a => a.outstanding_balance);
                    break;
                case "balance_desc":
                    accounts = accounts.OrderByDescending(a => a.outstanding_balance);
                    break;
                default:
                    accounts = accounts.OrderBy(a => a.account_number);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(accounts.ToPagedList(pageNumber, pageSize));
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.person_codeNavigation)
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(m => m.code == id);

            if (account == null) return NotFound();

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create(int personCode)
        {
            var account = new Account { person_code = personCode };
            return View(account);
        }

        // POST: Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Persons", new { id = account.person_code });
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return NotFound();
            return View(account);
        }

        // POST: Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Account account)
        {
            if (id != account.code) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Accounts.Any(e => e.code == account.code))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Details", "Persons", new { id = account.person_code });
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.person_codeNavigation)
                .FirstOrDefaultAsync(m => m.code == id);

            if (account == null) return NotFound();

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}