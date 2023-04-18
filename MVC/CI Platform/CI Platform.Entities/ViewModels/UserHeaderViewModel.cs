using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class UserHeaderViewModel
    {
        public string UserName { get; set; } = null!;
        public string isLoggedIn { get; set; } = null!;
        public long UserId { get; set; }
        public string profileImage { get; set; } = null!;
        public string userEmail { get; set; } = null!;
        [Required]
        public String Subject { get; set; } = null!;
        [Required]
        public String Message { get; set; } = null!;
        public List<CmsTable> cmsPages { get; set; } = new List<CmsTable>();
        public string Role { get; set; } = null!;
    }
}
