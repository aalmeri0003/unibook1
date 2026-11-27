using Microsoft.AspNetCore.Mvc;

namespace UniBook.Controllers
{
    // User Account page: shows the cookies and the session values
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.sessionId = HttpContext.Session.GetString("id");
            ViewBag.sessionUserName = HttpContext.Session.GetString("username");
            ViewBag.homeVisits = HttpContext.Session.GetInt32("homeVisits");
            ViewBag.lastBook = HttpContext.Session.GetString("lastBook");
            return View();
        }

        // Clear Session button: removes everything saved in the session
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearSession()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }
    }
}
