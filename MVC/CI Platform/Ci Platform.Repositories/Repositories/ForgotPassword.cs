using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Repositories
{
    public class ForgotPassword : Repository<User>, IForgotPassword
    {
        public ApplicationDbContext _context;
        public ForgotPassword(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public PasswordReset BindData(ForgotPasswordModel model)
        {
            PasswordReset passwordReset = new PasswordReset();
            passwordReset.Email = model.email;
            passwordReset.Token = model.token;
            return passwordReset;
        }
        public string GenerateToken()
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


            var fromEmail = new MailAddress("demo90720@gmail.com");
            var toEmail = new MailAddress(model.email);
            var fromEmailPassword = "vpomharwnqswzdao";
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


    }
}
