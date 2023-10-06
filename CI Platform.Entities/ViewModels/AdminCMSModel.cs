using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminCMSModel
    {
        public List<CmsTable> cmsTables { get; set; } = new List<CmsTable>();
        public long CmsPageId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Slug is required")]
        public string Slug { get; set; } = null!;
        [Required(ErrorMessage = "Select status")]
        public int? Status { get; set; }
    }
}
