using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_web.Controllers
{
    public class UserController : Controller
    {
        public async Task<IActionResult> UserProfile()
        {
            var UserId = "";
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
                UserId = HttpContext.Session.GetString("UserId");
                ViewBag.UserId = UserId;
            }
            else
            {
                ViewBag.UserName = "Login";
            }
            return View();
        }
    }
}
