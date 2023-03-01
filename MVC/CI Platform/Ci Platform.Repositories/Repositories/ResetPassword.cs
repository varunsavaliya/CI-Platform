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
    public class ResetPassword : Repository<User>, IResetPassword
    {
        public ApplicationDbContext _context;
        public ResetPassword(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void ResetPass(PasswordResetModel model)
        {
            var x = _context.Users.FirstOrDefault(e => e.Email == model.Email);


            x.Password = model.password;


            _context.Users.Update(x);
            _context.SaveChanges();
            
        }


    }
}
