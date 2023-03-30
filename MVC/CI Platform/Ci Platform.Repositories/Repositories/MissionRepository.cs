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
    public class MissionRepository : SendInvite<MissionVolunteeringModel>, IMission
    {
        private readonly ApplicationDbContext _context;

        public MissionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
