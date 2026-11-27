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
            return View();
        }
    }
}
