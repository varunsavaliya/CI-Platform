using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Repositories
{
    public class AdminRepository : IAdmin
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<User> GetUsers()
        {
            return _context.Users.Where(user => user.Role == "User" && user.DeletedAt == null).ToList();
        }

        public AdminUserModel GetUserById(long userId)
        {
            User? user = _context.Users.Include(user => user.City).FirstOrDefault(user => user.UserId == userId);
            AdminUserModel model = new()
            {
                UserId = user.UserId,
                Name = user.FirstName,
                Surname = user.LastName,
                Email = user.Email,
                Password = user.Password,
                CountryId = user.CountryId == 0 ? null : user.CountryId,
                CityId = user.CityId == 0 ? null : user.CityId,
                employeeId = user.EmployeeId,
                Status = user.Status,
                department = user.Department,
                MyProfile = user.ProfileText
            };
            if (user.CountryId != 0 || user.CountryId != null)
            {
                model.cityList = _context.Cities.Where(city => city.CountryId == user.CountryId).ToList();
            }
            return model;
        }
        public async Task<string> DeleteUserById(long userId)
        {
            User? userTobeRemoved = _context.Users.FirstOrDefault(user => user.UserId == userId);
            userTobeRemoved.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return "User ";
        }
        public bool IsUserExists(string email)
        {
            return _context.Users.Any(user => user.Email == email);
        }
        public bool IsUserExists(string email, long userId)
        {
            return _context.Users.Any(u => u.UserId != userId && u.Email == email); ;
        }


        public async Task AddUser(AdminUserModel model)
        {
            User user = new()
            {
                FirstName = model.Name,
                LastName = model.Surname,
                Email = model.Email,
                Password = model.Password,
                CountryId = model.CountryId == 0 ? null : model.CountryId,
                CityId = model.CityId == 0 ? null : model.CityId,
                EmployeeId = model.employeeId,
                Status = model.Status,
                Department = model.department,
                ProfileText = model.MyProfile
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(AdminUserModel model)
        {
            User? user = _context.Users.FirstOrDefault(user => user.UserId == model.UserId);
            if (user != null)
            {
                user.FirstName = model.Name;
                user.LastName = model.Surname;
                user.Email = model.Email;
                user.Password = model.Password;
                user.CountryId = model.CountryId == 0 || model.CountryId == null ? null : model.CountryId;
                user.CityId = model.CityId == 0 || model.CityId == null ? null : model.CityId;
                user.EmployeeId = model.employeeId;
                user.Status = model.Status;
                user.Department = model.department;
                user.ProfileText = model.MyProfile;
                user.UpdatedAt = DateTime.Now;
            };
            await _context.SaveChangesAsync();
        }

        public List<CmsTable> GetCMSList()
        {
           return _context.CmsTables.ToList();
        }

        public  AdminCMSModel GetCMSById(long cmsId)
        {
            CmsTable? cmsTable = _context.CmsTables.Find(cmsId);
            AdminCMSModel model = new()
            {
                CmsPageId = cmsTable.CmsPageId,
                Title = cmsTable.Title,
                Description = cmsTable.Description,
                Slug = cmsTable.Slug,
                Status = cmsTable.Status,
            };
            return model;
        }

        public async Task AddCMS(AdminCMSModel model)
        {
            CmsTable cms = new()
            {
                Title = model.Title,
                Description = model.Description,
                Slug = model.Slug,
                Status = model.Status,                
            };

            await _context.CmsTables.AddAsync(cms);
            await _context.SaveChangesAsync();
        }

        public bool IsCMSExists(string slug)
        {
            return _context.CmsTables.Any(cms => cms.Slug == slug); ;
        }

        public bool IsCMSExists(string slug, long cmsPageId)
        {
            return _context.CmsTables.Any(cms => cms.CmsPageId != cmsPageId && cms.Slug == slug); ;
        }


        public async Task UpdateCMS(AdminCMSModel model)
        {
            CmsTable? cms = _context.CmsTables.FirstOrDefault(cms => cms.CmsPageId == model.CmsPageId);
            if (cms != null)
            {
                cms.Title = model.Title;
                cms.Description = model.Description;
                cms.Slug = model.Slug;
                cms.Status = model.Status;
                cms.UpdatedAt = DateTime.Now;
            };
            await _context.SaveChangesAsync();
        }


        public List<Mission> GetMissionList()
        {
            List<Mission> missions = _context.Missions.ToList();
            return missions;
        }

    }
}
