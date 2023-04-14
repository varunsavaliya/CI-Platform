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
            return _context.Users.Where(user => user.RoleId == 1 && user.DeletedAt == null).ToList();
        }

        public User GetUserById(long userId)
        {
            User? user = _context.Users.Include(user => user.City).FirstOrDefault(user => user.UserId == userId);
            return user;
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
            return _context.Users.Any(user => user.UserId != userId && user.Email == email);
        }


        public async Task AddUser(AdminUserModel model)
        {
            try
            {

            User user = new()
            {
                FirstName = model.Profile.Name,
                LastName = model.Profile.Surname,
                Email = model.Email,
                Password = model.Password,
                CountryId = model.CountryId == 0 ? null : model.CountryId,
                CityId = model.CityId == 0 ? null : model.CityId,
                EmployeeId = model.Profile.employeeId,
                Status = model.Status,
                Department = model.Profile.department,
                ProfileText = model.Profile.MyProfile
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        } 
        
        public async Task UpdateUser(AdminUserModel model)
        {
            User? user = _context.Users.FirstOrDefault(user => user.UserId == model.UserId);
            if (user != null)
            {
                user.FirstName = model.Profile.Name;
                user.LastName = model.Profile.Surname;
                user.Email = model.Email;
                user.Password = model.Password;
                user.CountryId = model.CountryId;
                user.CityId = model.CityId;
                user.EmployeeId = model.Profile.employeeId;
                user.Status = model.Status;
                user.Department = model.Profile.department;
                user.ProfileText = model.MyProfile;
                user.UpdatedAt = DateTime.Now;
            };
            await _context.SaveChangesAsync();
        }
    }
}
