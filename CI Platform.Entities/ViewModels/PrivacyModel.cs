using CI_Platform.Entities.DataModels;

namespace CI_Platform.Entities.ViewModels
{
    public class PrivacyModel
    {
        public List<CmsTable> cmsPages { get; set; } = new List<CmsTable>();
    }
}
