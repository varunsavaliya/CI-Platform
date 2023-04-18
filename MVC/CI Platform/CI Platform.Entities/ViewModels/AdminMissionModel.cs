using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminMissionModel
    {
        public List<Mission> Missions { get; set; } = new List<Mission>();
        public long MissionId { get; set; }

        public long ThemeId { get; set; }

        public long CityId { get; set; }

        public long CountryId { get; set; }

        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string MissionType { get; set; } = null!;

        public int? Status { get; set; }

        public string? OrganizationName { get; set; }

        public string? OrganizationDetail { get; set; }

        public string? Availability { get; set; }

        public int? TotalSeats { get; set; }
    }
}
