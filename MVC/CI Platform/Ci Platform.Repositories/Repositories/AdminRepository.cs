using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Repositories
{
    public class AdminRepository: IAdmin
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<User> GetUsers()
        {
            return _context.Users.Where(user => user.RoleId == 1).ToList();
        }

    }
}
