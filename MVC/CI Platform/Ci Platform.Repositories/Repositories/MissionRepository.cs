using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;

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
