using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform_web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.Common;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CI_Platform_web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdmin _admin;
        private readonly IFilters _filters;

        public AdminController(IAdmin admin, IFilters filters)
        {
            _admin = admin;
            _filters = filters;
        }

        public IAdmin Admin { get; }

        public async Task<IActionResult> AdminUser()
        {
            AdminUserModel vm = new();

            vm.users = _admin.GetUsers(); // can throw specific exception like DbException
            List<Country> countryList = await _filters.GetCountriesAsync();
            vm.countryList = countryList;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AdminUser(AdminUserModel model)
        {
            if (model.UserId == 0)
            {
                if (_admin.IsUserExists(model.Email))
                {
                    TempData["Message"] = "User " + Constants.existsMessage;
                    TempData["Icon"] = "error";
                }
                else
                {
                    await _admin.AddUser(model);
                    TempData["Message"] = "User " + Constants.addMessage;
                    TempData["Icon"] = "success";
                }
            }
            else
            {
                if (_admin.IsUserExists(model.Email, model.UserId))
                {
                    TempData["Message"] = "User " + Constants.existsMessage;
                    TempData["Icon"] = "error";
                }
                else
                {
                    await _admin.UpdateUser(model);
                    TempData["Message"] = "User " + Constants.updateMessage;
                    TempData["Icon"] = "success";
                }
            }
            return RedirectToAction("AdminUser");
        }

        public IActionResult GetUserDataById(long id)
        {
            User user = _admin.GetUserById(id);
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return Json(user, options);

        }

        public async Task<IActionResult> DeleteUser(long id)
        {
            string message = await _admin.DeleteUserById(id);
            return Json(message + Constants.deleteMessage);
        }
    }
}
