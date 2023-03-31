using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Numerics;

namespace CI_Platform_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthentication _Authentication;
        private readonly ApplicationDbContext _context;


        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, IAuthentication authentication)
        {
            _context = context;
            _logger = logger;
            _Authentication = authentication;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult Index(User obj, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.IsRegistered(obj))
                {

                    if (_Authentication.ComparePassword(obj))
                    {
                        // creating username for session to show on profile field
                        var sessionUser = _context.Users.Where(a => a.Email == obj.Email).FirstOrDefault();
                        HttpContext.Session.SetString("UserName", sessionUser.FirstName + " " + sessionUser.LastName);
                        var userId = sessionUser.UserId;
                        var session = HttpContext.Session;

                        // Retrieve the BigInt value from the session
                        HttpContext.Session.SetString("UserId", (sessionUser.UserId).ToString());
                        HttpContext.Session.SetString("IsLoggedIn", "True");
                        // Redirect the user back to the previous page if returnUrl is present, otherwise to the home page
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("UserProfile", "User");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Password is incorrect");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email is incorrect");
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
                var data = _Authentication.BindData(model);

                if (_Authentication.IsRegistered(data))
                {

                    var token = _Authentication.GenerateToken();
                    var PasswordResetLink = Url.Action("Reset_Password", "Home", new { Email = model.email, Token = token }, Request.Scheme);

                    _Authentication.SendMail(token, PasswordResetLink, model);

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
                _Authentication.ResetPass(model);
                model.PasswordChanged = true;
                //return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("Token", "Reset Passwordword Link is Invalid");
            }
            return View(model);
        }

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
                if (_Authentication.IsRegistered(obj))
                {
                    _Authentication.Add(obj);
                    _Authentication.Save();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email already exists, please login!");
                }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("IsLoggedIn");
            return RedirectToAction("Index", "Home");
        }


        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}