using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class InputData
    {
        public string selectedCountry { get; set; }
        public string selectedCities { get; set; }
        public string selectedThemes { get; set; }
        public string selectedSkills { get; set; }
        public string searchText { get; set; }
        public string selectedSortOption { get; set; }
        public string userId { get; set; }
        public int pageSize { get; set; }
        public int pageNo { get; set; }
    }

}
