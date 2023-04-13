using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CI_Platform_web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdmin _admin;
        public AdminController(IAdmin admin)
        {
            _admin = admin;
        }

        public IAdmin Admin { get; }

        public IActionResult User()
        {
            AdminUserModel vm = new();
            vm.users = _admin.GetUsers();
            return View(vm);
        }
    }
}
