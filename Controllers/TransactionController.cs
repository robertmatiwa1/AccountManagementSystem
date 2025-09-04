using AccountManagementSystem.Data;
using AccountManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace AccountManagementSystem.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["AmountSortParm"] = sortOrder == "Amount" ? "amount_desc" : "Amount";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var transactions = _context.Transactions.Include(t => t.account_codeNavigation).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                transactions = transactions.Where(t =>
                    (t.description != null && t.description.Contains(searchString)) ||
                    (t.account_codeNavigation != null && t.account_codeNavigation.account_number != null &&
                     t.account_codeNavigation.account_number.Contains(searchString)));
            }

            switch (sortOrder)
            {
                case "date_desc":
                    transactions = transactions.OrderByDescending(t => t.transaction_date);
                    break;
                case "Amount":
                    transactions = transactions.OrderBy(t => t.amount);
                    break;
                case "amount_desc":
                    transactions = transactions.OrderByDescending(t => t.amount);
                    break;
                default:
                    transactions = transactions.OrderBy(t => t.transaction_date);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(transactions.ToPagedList(pageNumber, pageSize));
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.account_codeNavigation)
                .FirstOrDefaultAsync(m => m.code == id);

            if (transaction == null) return NotFound();

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create(int accountCode)
        {
            // Check if account is closed
            var account = _context.Accounts.Find(accountCode);
            if (account != null && account.is_closed)
            {
                TempData["ErrorMessage"] = "Cannot add transactions to a closed account.";
                return RedirectToAction("Details", "Accounts", new { id = accountCode });
            }

            var transaction = new Transaction { 
                account_code = accountCode,
                transaction_date = DateTime.Now,
                capture_date = DateTime.Now
            };
            return View(transaction);
        }

        // POST: Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            // Business Rule: Transaction date cannot be in the future
            if (transaction.transaction_date > DateTime.Now)
            {
                ModelState.AddModelError("transaction_date", "Transaction date cannot be in the future.");
            }

            // Business Rule: Amount cannot be zero
            if (transaction.amount == 0)
            {
                ModelState.AddModelError("amount", "Transaction amount cannot be zero.");
            }

            if (ModelState.IsValid)
            {
                // Business Rule: Cannot post to closed accounts
                var account = await _context.Accounts.FindAsync(transaction.account_code);
                if (account != null && account.is_closed)
                {
                    ModelState.AddModelError("", "Cannot add transactions to a closed account.");
                    return View(transaction);
                }

                transaction.capture_date = DateTime.Now;
                _context.Add(transaction);

                // Update account balance
                if (account != null)
                {
                    account.outstanding_balance += transaction.amount;
                    _context.Update(account);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Accounts", new { id = transaction.account_code });
            }
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return NotFound();

            // Business Rule: Cannot edit transactions in closed accounts
            var account = await _context.Accounts.FindAsync(transaction.account_code);
            if (account != null && account.is_closed)
            {
                TempData["ErrorMessage"] = "Cannot edit transactions in a closed account.";
                return RedirectToAction("Details", "Accounts", new { id = transaction.account_code });
            }

            return View(transaction);
        }

        // POST: Transactions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Transaction transaction)
        {
            if (id != transaction.code) return NotFound();

            // Business Rule: Transaction date cannot be in the future
            if (transaction.transaction_date > DateTime.Now)
            {
                ModelState.AddModelError("transaction_date", "Transaction date cannot be in the future.");
            }

            // Business Rule: Amount cannot be zero
            if (transaction.amount == 0)
            {
                ModelState.AddModelError("amount", "Transaction amount cannot be zero.");
            }

            if (ModelState.IsValid)
            {
                // Business Rule: Cannot edit transactions in closed accounts
                var account = await _context.Accounts.FindAsync(transaction.account_code);
                if (account != null && account.is_closed)
                {
                    ModelState.AddModelError("", "Cannot edit transactions in a closed account.");
                    return View(transaction);
                }

                try
                {
                    // Preserve original capture date
                    var originalTransaction = await _context.Transactions.AsNoTracking()
                        .FirstOrDefaultAsync(t => t.code == id);
                    
                    if (originalTransaction != null)
                    {
                        transaction.capture_date = originalTransaction.capture_date;
                    }

                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Transactions.Any(e => e.code == transaction.code))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Details", "Accounts", new { id = transaction.account_code });
            }
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.account_codeNavigation)
                .FirstOrDefaultAsync(m => m.code == id);

            if (transaction == null) return NotFound();

            // Business Rule: Cannot delete transactions from closed accounts
            var account = await _context.Accounts.FindAsync(transaction.account_code);
            if (account != null && account.is_closed)
            {
                TempData["ErrorMessage"] = "Cannot delete transactions from a closed account.";
                return RedirectToAction("Details", "Accounts", new { id = transaction.account_code });
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                // Business Rule: Cannot delete transactions from closed accounts
                var account = await _context.Accounts.FindAsync(transaction.account_code);
                if (account != null && account.is_closed)
                {
                    TempData["ErrorMessage"] = "Cannot delete transactions from a closed account.";
                    return RedirectToAction("Details", "Accounts", new { id = transaction.account_code });
                }

                // Adjust balance
                if (account != null)
                {
                    account.outstanding_balance -= transaction.amount;
                    _context.Update(account);
                }

                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}