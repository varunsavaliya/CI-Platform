using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform_web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace CI_Platform_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly UserManager<IdentityUser> userManager;
        //private readonly SignInManager<IdentityUser> signInManager;
        private ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger /*UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager*/)
        {
            _context = context;
            _logger = logger;
            //this.userManager = userManager;
            //this.signInManager = signInManager;
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
        public IActionResult Forgot_password(ForgotPasswordModel model)
        {


            if (ModelState.IsValid)
            {
                var data = _context.Users.Where(e => e.Email == model.email).FirstOrDefault();

                if (data != null)
                {

                    Random random = new Random();

                    int capitalCharCode = random.Next(65, 91);
                    char randomCapitalChar = (char)capitalCharCode;


                    int randomint = random.Next();


                    int SmallcharCode = random.Next(97, 123);
                    char randomChar = (char)SmallcharCode;

                    String token = "";
                    token += randomCapitalChar.ToString();
                    token += randomint.ToString();
                    token += randomChar.ToString();


                    var PasswordResetLink = Url.Action("Reset_Password", "Home", new { Email = model.email, Token = token }, Request.Scheme);

                    var ResetPasswordInfo = new PasswordReset()
                    {
                        Email = model.email,
                        Token = token
                    };
                    _context.PasswordResets.Add(ResetPasswordInfo);
                    _context.SaveChanges();


                    var fromEmail = new MailAddress("demo90720@gmail.com");
                    var toEmail = new MailAddress(model.email);
                    var fromEmailPassword = "Demodemo";
                    string subject = "Reset Password";
                    string body = PasswordResetLink;

                    var smtp = new SmtpClient
                    {

                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                    };

                    MailMessage message = new MailMessage(fromEmail, toEmail);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    smtp.Send(message);

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
                var x = _context.Users.FirstOrDefault(e => e.Email == model.Email);


                x.Password = model.password;


                _context.Users.Update(x);
                _context.SaveChanges();

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
                var chkMail = _context.Users.Any(e => e.Email == model.Email);
                if (!chkMail)
                {

                _context.Users.Add(obj);
                _context.SaveChanges();
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