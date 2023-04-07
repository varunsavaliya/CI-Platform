using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Ci_Platform.Repositories.Repositories
{
    public class UserProfile : IUserProfile
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserProfile(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserProfileModel> GetUser(long id)
        {
            User user = _context.Users.Include(u => u.City).Include(u => u.Country).Where(u => u.UserId == id).FirstOrDefault();
            var vm = new UserProfileModel()
            {
                Name = user.FirstName,
                Surname = user.LastName,
                employeeId = user.EmployeeId == "null" ? "" : user.EmployeeId,
                title = user.Title == "null" ? "" : user.Title,
                department = user.Department == "null" ? "" : user.Department,
                MyProfile = user.ProfileText == "null" ? "" : user.ProfileText,
                whyIVolunteer = user.WhyIVolunteer == "null" ? "" : user.WhyIVolunteer,
                Country = (long)user.CountryId,
                City = user.CityId,
                LinkedIn = user.LinkedInUrl,
                ProfileImageName = user.Avatar,
            };
            return vm;
        }

        public async Task<List<UserSkill>> GetUserSkills(long id)
        {
            List<UserSkill> userSkills = _context.UserSkills.Include(u => u.Skill).Where(us => us.UserId == id).ToList();
            return userSkills;
        }
        public async Task UpdateUserDetails(long id, UserProfileModel model)
        {
            var user = _context.Users.Where(u => u.UserId == id).FirstOrDefault();

            // profile image
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                // Check if the user already has a profile image
                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    // Delete old profile image from the profile images folder
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfileImages", user.Avatar);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    user.Avatar = null;
                    await _context.SaveChangesAsync();
                }

                // Save new profile image to the profile images folder with a unique file name
                var fileExtension = Path.GetExtension(model.ProfileImage.FileName);
                var fileName = id + "_ProfileImage" + fileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profileimages", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                // Update user's profile record in the database with the new file name
                user.Avatar = fileName;
                await _context.SaveChangesAsync();

            _httpContextAccessor.HttpContext.Session.SetString("profileImage", user.Avatar);
            }

            if(model.Name != null && user.FirstName != model.Name)
            {
                user.FirstName = model.Name;

            }
            if (model.Surname != null && user.LastName != model.Surname)
            {
                user.LastName = model.Surname;
            }
            _httpContextAccessor.HttpContext.Session.SetString("UserName", model.Name +" "+ model.Surname);

            if (model.employeeId != null)
            {
                user.EmployeeId = model.employeeId;
            }
            if (model.title != null && user.Title != model.title)
            {
                user.Title = model.title;
            }
            if (model.department != null && user.Department != model.department)
            {
                user.Department = model.department;
            }
            if (model.MyProfile != null && user.ProfileText != model.MyProfile)
            {
                user.ProfileText = model.MyProfile;
            }
            if (model.whyIVolunteer != null && user.WhyIVolunteer != model.whyIVolunteer)
            {
                user.WhyIVolunteer = model.whyIVolunteer;
            }
            if (model.City != 0 && user.CityId != model.City)
            {
                user.CityId = model.City;
            }
            if (model.Country != 0 && user.CountryId != model.Country)
            {
                user.CountryId = model.Country;
            }
            if (model.LinkedIn != null && user.LinkedInUrl != model.LinkedIn)
            {
                user.LinkedInUrl = model.LinkedIn;
            }
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            // user skills
            if (model.selectedSkills != null)
            {
                string[] selectedSkills = model.selectedSkills.Split(',');

                // Retrieve the user's existing skills from the database
                var existingSkills = _context.UserSkills.Where(x => x.UserId == id).ToList();

                // Delete the user's existing skills from the database
                _context.UserSkills.RemoveRange(existingSkills);
                await _context.SaveChangesAsync();


                // Insert the selected skills into the database
                foreach (var skill in selectedSkills)
                {
                    var userSkill = new UserSkill
                    {
                        UserId = id,
                        SkillId = int.Parse(skill)
                    };
                  await _context.UserSkills.AddAsync(userSkill);
                }
                await _context.SaveChangesAsync();
            }

        }
    }
}
