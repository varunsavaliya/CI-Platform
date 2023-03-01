using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform_web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace CI_Platform_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRegister _register;
        private readonly ILogin _login;
        private readonly IForgotPassword _forgotPassword;
        private readonly IResetPassword _passwordReset;
        private readonly ApplicationDbContext _context;


        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, IRegister register, ILogin login, IForgotPassword forgotPassword, IResetPassword passwordReset)
        {
            _context = context;
            _logger = logger;
            _register = register;
            _login = login;
            _forgotPassword = forgotPassword;
            _passwordReset = passwordReset;
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
                //var obj2 = _userAuthentication.chkUser(obj);
                if (_login.ComparePassword(obj))
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
        public IActionResult Forgot_password(ForgotPasswordModel model)
        {


            if (ModelState.IsValid)
            {
                //var data = _context.Users.Where(e => e.Email == model.email).FirstOrDefault();
                var data = _forgotPassword.BindData(model);

                if (_forgotPassword.IsRegistered(data))
                {

                    var token = _forgotPassword.GenerateToken();
                    var PasswordResetLink = Url.Action("Reset_Password", "Home", new { Email = model.email, Token = token }, Request.Scheme);

                    _forgotPassword.SendMail(token, PasswordResetLink, model);

                    ModelState.Clear();
                    model.emailSent = true;

                }
                else
                {
                    ModelState.AddModelError("Email", "Email is not Registered");

                }
            }
            else
            {
                return View(model);
            }
            return View(model);
        }
        public IActionResult Reset_Password(String email, String token)
        {

            PasswordResetModel validation = new PasswordResetModel()
            {
                Email = email,
                Token = token
            };


            return View(validation);

        }

        [HttpPost]
        public IActionResult Reset_Password(PasswordResetModel model)
        {

            var ResetPasswordData = _context.PasswordResets.Any(e => e.Email == model.Email && e.Token == model.Token);


            if (ResetPasswordData)
            {
                _passwordReset.ResetPass(model);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("Token", "Reset Passwordword Link is Invalid");
            }
            return View(model);
        }

        //public IActionResult Reset_Password()
        //{
        //    return View();
        //} 

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(User obj, RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                //var chkMail = obj.Email == model.Email;
                //var chkMail = _context.Users.Any(e => e.Email == model.Email);
                
                if (_register.IsRegistered(obj))
                {
                _register.Add(obj);
                _register.Save();
                return RedirectToAction("LandingPage", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email already exists, please login!");
                }
            }
            return View(model);
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