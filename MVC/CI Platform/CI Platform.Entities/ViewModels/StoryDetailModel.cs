using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class StoryDetailModel
    {
        public Story StoryDetail { get; set; } = new();
        public List<User> UserList { get; set; } = new();
    }
}
