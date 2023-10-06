using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_web.Views.ViewComponents
{
    public class UserHeaderViewComponent : ViewComponent
    {
        private readonly IFilters _filter;

        public UserHeaderViewComponent(IFilters filter)
        {
            _filter = filter;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName)
        {
            string UserName = "";
            string IsLoggedIn = "";
            long UserId = 0;
            string profileImage = "";
            string email = "";
            List<CmsTable> cmsTables = new();
            if (HttpContext.Session.GetString("UserName") != null)
            {
                UserName = HttpContext.Session.GetString("UserName");
                ViewBag.UserName = UserName;
                IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
                UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
                profileImage = HttpContext.Session.GetString("profileImage");
                email = HttpContext.Session.GetString("userEmail");
                ViewBag.Email = email;
            }
            else
            {
                UserName = "Login";
                IsLoggedIn = "False";
                UserId = 0;
            }
            UserHeaderViewModel viewModel = new()
            {
                UserName = UserName,
                isLoggedIn = IsLoggedIn,
                UserId = UserId,
                profileImage = profileImage,
                userEmail = email,
                cmsPages = await _filter.GetCmsTables(),
            };
            return View(viewName, viewModel);
        }
    }
}
