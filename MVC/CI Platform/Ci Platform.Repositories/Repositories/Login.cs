using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Repositories
{
    public class Login: Repository<User>, ILogin
    {
        private ApplicationDbContext _context;

        public Login(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
