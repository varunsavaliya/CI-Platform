using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult User()
        {
            return View();
        }
    }
}
