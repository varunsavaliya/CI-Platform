using CI_Platform.Entities.DataModels;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModels
{
    public class BannerModel
    {
        public List<Banner> Banners { get; set; } = new List<Banner>();
        public long BannerId { get; set; }
        public IFormFile? Image { get; set; } = null!;
        public string? BannerImageName { get; set; } = null!;
        [Required(ErrorMessage ="Enter text for image")]
        public string? Text { get; set; }
        [Required(ErrorMessage = "Enter sort order")]
        public int? SortOrder { get; set; }
    }
}
