using AccountManagementSystem.Data;
using AccountManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace AccountManagementSystem.Controllers
{
    public class PersonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Persons
        public IActionResult Index(string sortOrder, string idNumber, string surname, string accountNumber, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SurnameSortParm"] = sortOrder == "Surname" ? "surname_desc" : "Surname";

            // Store current search values in ViewData to preserve them in the form
            ViewData["CurrentIdNumber"] = idNumber;
            ViewData["CurrentSurname"] = surname;
            ViewData["CurrentAccountNumber"] = accountNumber;

            // Check if any search parameter has value
            bool hasSearch = !string.IsNullOrEmpty(idNumber) || !string.IsNullOrEmpty(surname) || !string.IsNullOrEmpty(accountNumber);

            var persons = _context.Persons
                .Include(p => p.Accounts)
                .AsQueryable();

            // Apply filters if provided
            if (!string.IsNullOrEmpty(idNumber))
            {
                persons = persons.Where(p => p.id_number != null && p.id_number.Contains(idNumber));
            }

            if (!string.IsNullOrEmpty(surname))
            {
                persons = persons.Where(p => p.surname != null && p.surname.Contains(surname));
            }

            if (!string.IsNullOrEmpty(accountNumber))
            {
                persons = persons.Where(p => p.Accounts.Any(a => 
                    a.account_number != null && 
                    a.account_number.Contains(accountNumber)
                ));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    persons = persons.OrderByDescending(p => p.name);
                    break;
                case "Surname":
                    persons = persons.OrderBy(p => p.surname);
                    break;
                case "surname_desc":
                    persons = persons.OrderByDescending(p => p.surname);
                    break;
                default:
                    persons = persons.OrderBy(p => p.name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(persons.ToPagedList(pageNumber, pageSize));
        }

        // GET: Persons/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var person = await _context.Persons
                .Include(p => p.Accounts)
                .FirstOrDefaultAsync(m => m.code == id);

            if (person == null) return NotFound();

            return View(person);
        }

        // GET: Persons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Persons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: Persons/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null) return NotFound();
            return View(person);
        }

        // POST: Persons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Person person)
        {
            if (id != person.code) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Persons.Any(e => e.code == person.code))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: Persons/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.code == id);

            if (person == null) return NotFound();

            return View(person);
        }

        // POST: Persons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Export to CSV action (if needed)
        public IActionResult ExportCsv()
        {
            // Your export logic here
            return Content("Export functionality to be implemented");
        }
    }
}