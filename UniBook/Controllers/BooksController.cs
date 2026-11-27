using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniBook.Data;
using UniBook.Models;

namespace UniBook.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books  (RETRIEVE: show all books)
        public async Task<IActionResult> Index()
        {
            return _context.Book != null ?
                        View(await _context.Book.OrderBy(b => b.faculty).ThenBy(b => b.courseCode).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Book'  is null.");
        }

        // GET: Books/SearchForm
        public IActionResult SearchForm()
        {
            // read the last search from the cookie (empty the first time)
            ViewBag.LastSearch = Request.Cookies["lastSearch"];
            return View();
        }

        // POST: Books/ShowSearchFormResult  (SEARCH by book name or course code)
        public async Task<IActionResult> ShowSearchFormResult(string SearchBook)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Book' is null.");
            }

            if (string.IsNullOrEmpty(SearchBook))
            {
                return RedirectToAction(nameof(Index));
            }

            // save what the user searched for in a cookie for 15 days
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(15),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("lastSearch", SearchBook, options);

            var filteredBooks = await _context.Book
                                         .Where(j => j.title.Contains(SearchBook) || j.courseCode.Contains(SearchBook))
                                         .ToListAsync();

            ViewBag.SearchBook = SearchBook;
            return View("Index", filteredBooks);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FirstOrDefaultAsync(m => m.id == id);
            if (book == null)
            {
                return NotFound();
            }

            // remember the last book the user opened (shown on the User Account page)
            HttpContext.Session.SetString("lastBook", book.courseCode + " - " + book.title);

            return View(book);
        }

        // GET: Books/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create  (INSERT a new book, logged in users only)
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,courseCode,title,faculty,version,newPrice,usedPrice")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5  (UPDATE a book, logged in users only)
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,courseCode,title,faculty,version,newPrice,usedPrice")] Book book)
        {
            if (id != book.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FirstOrDefaultAsync(m => m.id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5  (DELETE a book, logged in users only)
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return (_context.Book?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
