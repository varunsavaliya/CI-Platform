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
        public string UserName { get; set; }
        public string isLoggedIn { get; set; }
        public long UserId { get; set; }
        public string profileImage { get; set; }
        public string userEmail { get; set; }
        [Required]
        public String Subject { get; set; }
        [Required]
        public String Message { get; set; }
    }
}
