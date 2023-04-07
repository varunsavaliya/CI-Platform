using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace Ci_Platform.Repositories.Repositories
{
    public class SendInvite<T>: ISendInvite<T> where T : class
    {
        public readonly ApplicationDbContext _context;

        public SendInvite(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SendEmailInvite(long ToUserId, long Id, long FromUserId, String link, T viewmodel)
        {
            var receiver = await _context.Users.Where(u => u.UserId == ToUserId).FirstOrDefaultAsync();
            var Sender = await _context.Users.Where(su => su.UserId == FromUserId).FirstOrDefaultAsync();

            var fromEmail = new MailAddress("akshayghadiya28@gmail.com");
            var toEmail = new MailAddress(receiver.Email);
            var fromEmailPassword = "dmsmefwcumhbtthp";
            string subject = "";
            string body = "";
            if (viewmodel is MissionVolunteeringModel)
            {
                var missionInvite = new MissionInvite()
                {
                    FromUserId = FromUserId,
                    ToUserId = ToUserId,
                    MissionId = Id,
                };

                _context.MissionInvites.Add(missionInvite);
                await _context.SaveChangesAsync();



                 subject = "Mission Invitation";
                 body = "You Have Recieved Mission Invitation From " + Sender.FirstName + " " + Sender.LastName + " For:\n\n" + link;
            }
            else if(viewmodel is StoryDetailModel)
            {
                var storyInvite = new StoryInvite()
                {
                    FromUserId = FromUserId,
                    ToUserId = ToUserId,
                    StoryId = Id,
                };

                _context.StoryInvites.Add(storyInvite);
                await _context.SaveChangesAsync();


                 subject = "Story Invitation";
                 body = "You Have Recieved Story Invitation From " + Sender.FirstName + " " + Sender.LastName + " For:\n\n" + link;
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            var message = new MailMessage(fromEmail, toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            await smtp.SendMailAsync(message);
        }
    }
}
