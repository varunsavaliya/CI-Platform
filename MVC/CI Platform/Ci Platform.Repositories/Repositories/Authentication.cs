using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;

namespace Ci_Platform.Repositories.Repositories
{
    public class Authentication : Repository<User>, IAuthentication
    {
        public readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Authentication(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public string GenerateToken()
        {

            Random random = new();

            int capitalCharCode = random.Next(65, 91);
            char randomCapitalChar = (char)capitalCharCode;


            int randomint = random.Next();


            int SmallcharCode = random.Next(97, 123);
            char randomChar = (char)SmallcharCode;

            String token = "";
            token += randomCapitalChar.ToString();
            token += randomint.ToString();
            token += randomChar.ToString();

            return token;
        }
        public void SendMail(string token, string PasswordResetLink, ForgotPasswordModel model)
        {
            var ResetPasswordInfo = new PasswordReset()
            {
                Email = model.email,
                Token = token
            };
            _context.Add(ResetPasswordInfo);
            _context.SaveChanges();


            var fromEmail = new MailAddress("akshayghadiya28@gmail.com");
            var toEmail = new MailAddress(model.email);
            var fromEmailPassword = "dmsmefwcumhbtthp";
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
        }

        public void ResetPass(PasswordResetModel model)
        {
            var x = _context.Users.FirstOrDefault(e => e.Email == model.Email);
            x.Password = model.password;
            _context.Users.Update(x);
            _context.SaveChanges();
        }

        public async Task<bool> IsPasswordResetDataExist(PasswordResetModel model)
        {
            return _context.PasswordResets.Any(e => e.Email == model.Email && e.Token == model.Token);
        }


        public bool comparePass(long UserId, string pass)
        {
            return _context.Users.Any(u => u.UserId == UserId && u.Password == pass);
        }
        public void ResetPassword(long UserId, string pass)
        {
            var user = _context.Users.Where(u => u.UserId == UserId).FirstOrDefault();
            user.Password = pass;
            _context.SaveChanges();
        }
        public void SetSession(string email)
        {
            User? user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("UserName", user.FirstName + " " + user.LastName);
            var userId = user.UserId;
            session.SetString("UserId", (user.UserId).ToString());
            session.SetString("IsLoggedIn", "True");
            session.SetString("profileImage", user.Avatar == null ? "" : user.Avatar);
            session.SetString("userEmail", user.Email);
        }
        public void DestroySession()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Remove("UserName");
            session.Remove("UserId");
            session.Remove("IsLoggedIn");
            session.Remove("profileImage");
            session.Remove("userEmail");
        }

        public User GetUser(long userId)
        {
            return _context.Users.Find(userId);
        }


        public async Task AddToContactUs(UserHeaderViewModel contactUs)
        {
            var user = _context.Users.Where(u => u.Email == contactUs.userEmail).FirstOrDefault();
            ContactU data = new ContactU()
            {
                UserId = user.UserId,
                Subject = contactUs.Subject,
                Message = contactUs.Message,
            };

            await _context.ContactUs.AddAsync(data);
            await _context.SaveChangesAsync();
        }

    }
}
