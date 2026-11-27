using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UniBook.Models;

namespace UniBook.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // last visit cookie: read the old one (if any) before saving the new one
            ViewBag.LastVisit = Request.Cookies["lastVisit"];
            SetCookies("lastVisit", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));

            // create a session id only the first time, so it stays the same during the visit
            if (HttpContext.Session.GetString("id") == null)
            {
                SetSession("id", Guid.NewGuid().ToString());
            }

            // count how many times the home page was opened in this session
            int homeVisits = HttpContext.Session.GetInt32("homeVisits") ?? 0;
            homeVisits = homeVisits + 1;
            HttpContext.Session.SetInt32("homeVisits", homeVisits);
            ViewBag.HomeVisits = homeVisits;

            // Check if the user is authenticated
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // User is logged in, get their username
                SetCookies("userName", User.Identity.Name ?? "");
                // create a session with username
                SetSession("username", User.Identity.Name ?? "");
            }
            else
            {
                // User is not logged in, set a cookie with "Guest"
                SetCookies("userName", "guest");
                // create a session with username
                SetSession("username", "guest");
            }

            // Get the browser type
            SetCookies("browserName", Request.Headers["User-Agent"].ToString());

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        // saves a cookie in the user's browser for 15 days
        private void SetCookies(string cookieName, string cookieValue)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(15), // Cookie expires in 15 days
                HttpOnly = true,                    // Prevent JavaScript access to the cookies
                Secure = true,                      // Use Secure flag (HTTPS)
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append(cookieName, cookieValue, options);
        }

        // saves a value in the session (on the server)
        private void SetSession(string key, string value)
        {
            HttpContext.Session.SetString(key, value);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
