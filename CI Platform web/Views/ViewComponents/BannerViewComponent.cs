using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_web.Views.ViewComponents
{
    public class BannerViewComponent : ViewComponent
    {
        private readonly IAdmin _admin;

        public BannerViewComponent(IAdmin admin)
        {
            _admin = admin;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            BannerModel model = new()
            {
                Banners = await _admin.GetBannerList(),
            };
            return View(model);
        }
    }
}
