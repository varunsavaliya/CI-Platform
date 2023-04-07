using System.Threading.Tasks;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace CI_Platform_web.Views.ViewComponents
{
    public class UserHeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string viewName)
        {
            // Logic to get the current user's name and profile image from your session or database
            // You can also pass in any necessary parameters to this method to customize the header
            string UserName = "";
            string IsLoggedIn = "";
            long UserId = 0;
            string profileImage = "";
            string email = "";
            if (HttpContext.Session.GetString("UserName") != null)
            {
                UserName = HttpContext.Session.GetString("UserName");
                ViewBag.UserName = UserName;
                IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
                UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
                profileImage= HttpContext.Session.GetString("profileImage");
               email= HttpContext.Session.GetString("userEmail");
                ViewBag.Email = email;
            }
            else
            {
                UserName = "Login";
                IsLoggedIn = "False";
                UserId = 0;
            }

            // Create a new UserHeaderViewModel with the user's name and profile image
            UserHeaderViewModel viewModel = new()
            {
                UserName = UserName,
                isLoggedIn = IsLoggedIn,
                UserId = UserId,
                profileImage = profileImage,
                userEmail = email
            };

            // Return the ViewComponentResult with the UserHeaderViewModel
            return View(viewName, viewModel);
        }
    }
}
