using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminCommentsModel
    {
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
