using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform_web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_Platform_web.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        private ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(User obj)
        {
            if (ModelState.IsValid)
            {
                var obj2 = _context.Users.Where(a=>a.Email.Equals(obj.Email) && a.Password.Equals(obj.Password)).FirstOrDefault();
                if(obj2 != null)
                {
                    return RedirectToAction("LandingPage", "Home");
                }
            }
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult Forgot_Password()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Forgot_Password(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                model.emailSent = true;
            }
            return View(model);
        }

        public IActionResult Reset_Password()
        {
            return View();
        } 
        
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(User obj)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(obj);
                _context.SaveChanges();
                return RedirectToAction("LandingPage", "Home");
            }
            return View();
        }
        public IActionResult LandingPage()
        {
            return View();
        }

        public IActionResult ShareStory()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}