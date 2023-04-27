using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class InputData
    {
        public long CountryId { get; set; }
        public List<long> CityIds { get; set; } = new List<long>();
        public List<long> ThemeIds { get; set; } = new List<long>();
        public List<long> SkillIds { get; set; } = new List<long>();
        public string searchText { get; set; } = null!;
        public string Explore { get; set; } = null!;
        public string SortBy { get; set; } = "Newest";
        public string SortOrder { get; set; } = "Desc";
        public int pageSize { get; set; } = 9;
        public int pageNo { get; set; }
    }

}
