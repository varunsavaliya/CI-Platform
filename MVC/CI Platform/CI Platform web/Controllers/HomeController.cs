using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CI_Platform_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthentication _Authentication;


        public HomeController(ILogger<HomeController> logger, IAuthentication authentication, IConfiguration config)
        {
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
        public async Task<IActionResult> Index(User obj, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.IsRegistered(obj))
                {
                    if (_Authentication.ComparePassword(obj))
                    {
                        _Authentication.SetSession(obj.Email);

                        long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
                        // Redirect the user back to the previous page if returnUrl is present, otherwise to the home page
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            if (_Authentication.GetUserRole(UserId) == 1)
                                return RedirectToAction("LandingPage", "Mission");
                            else if(_Authentication.GetUserRole(UserId) == 2)
                                return RedirectToAction("User", "Admin");

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
            PasswordReset passwordReset = new PasswordReset()
            {
                Email = model.email,
                Token = model.token,
            };
            if (ModelState.IsValid)
            {
                if (_Authentication.IsRegistered(passwordReset))
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
        public async Task<IActionResult> Reset_Password(PasswordResetModel model)
        {
            if (await _Authentication.IsPasswordResetDataExist(model))
            {
                _Authentication.ResetPass(model);
                model.PasswordChanged = true;
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
            _Authentication.DestroySession();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult ChangePassword(String oldPass, String newPass)
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            if (_Authentication.comparePass(UserId, oldPass))
            {
                _Authentication.ResetPassword(Convert.ToInt64(UserId), newPass);
                return Ok(new { icon = "success", message = "Password Changed Successfully!!" });
            }
            return Ok(new { icon = "error", message = "Old password is incorrect" });
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(UserHeaderViewModel formData)
        {
            try
            {
                await _Authentication.AddToContactUs(formData);
                return Ok(new { icon = "success", message = "Thank  you for contacting us!!" });
            }
            catch (Exception ex)
            {
                // Log the error and return a response indicating an error occurred
                _logger.LogError(ex, "An error occurred while adding contact form data");
                return StatusCode(500, new { icon = "error", message = "An error occurred while submitting the form. Please try again later." });
            }
        }

        public IActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}