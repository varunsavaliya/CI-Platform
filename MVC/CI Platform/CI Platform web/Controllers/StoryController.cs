using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace CI_Platform_web.Controllers
{
    public class StoryController : Controller
    {
        private readonly IFilters _filters;
        private readonly IStory _story;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StoryController(IFilters filters, IStory story, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _filters = filters;
            _story = story;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> StoriesListing()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
                ViewBag.UserId = HttpContext.Session.GetString("UserId");
            }
            else
            {
                ViewBag.UserName = "Login";
            }
            StoriesListingModel viewModel = new StoriesListingModel
            {
                Country = await _filters.GetCountriesAsync(),
                Theme = await _filters.GetThemesAsync(),
                Skill = await _filters.GetSkillsAsyc()
            };
            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> StoriesListing(InputData inputData)
        {
            StoriesListingModel viewModel = new StoriesListingModel();
            // Extract the values from the inputData object
            string selectedCountry = inputData.selectedCountry;
            string selectedCities = inputData.selectedCities;
            string selectedThemes = inputData.selectedThemes;
            string selectedSkills = inputData.selectedSkills;
            string searchText = inputData.searchText;
            int pageSize = inputData.pageSize;
            int pageNo = inputData.pageNo;

            IConfigurationRoot _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Call the stored procedure
                SqlCommand command = new SqlCommand("spGetStory", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@countryId", SqlDbType.VarChar).Value = selectedCountry != null ? selectedCountry : null;
                command.Parameters.Add("@cityId", SqlDbType.VarChar).Value = selectedCities != null ? string.Join(",", selectedCities) : null;
                command.Parameters.Add("@themeId", SqlDbType.VarChar).Value = selectedThemes != null ? string.Join(",", selectedThemes) : null;
                command.Parameters.Add("@skillId", SqlDbType.NVarChar).Value = selectedSkills != null ? string.Join(",", selectedSkills) : null;
                command.Parameters.Add("@searchText", SqlDbType.VarChar).Value = searchText;
                command.Parameters.Add("@pageSize", SqlDbType.Int).Value = pageSize;
                command.Parameters.Add("@pageNo", SqlDbType.Int).Value = pageNo;
                SqlDataReader reader = command.ExecuteReader();

                // Read the results
                List<long> StoryIds = new List<long>();
                while (reader.Read())
                {
                    long totalStories = reader.GetInt32("TotalStories");
                    ViewBag.totalRecords = totalStories;
                }
                reader.NextResult();
                while (reader.Read())
                {
                    // read only Story id for comparing with story table
                    long StoryId = reader.GetInt64("story_id");
                    StoryIds.Add(StoryId);
                }
                viewModel.StoriesList = await _story.GetStories(StoryIds);
                connection.Close();
            }
            return PartialView("_StoryPartial", viewModel);
        }
        public async Task<IActionResult> ShareStory()
        {
            var UserId = "";
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
                UserId = HttpContext.Session.GetString("UserId");
                ViewBag.UserId = UserId;
            }
            else
            {
                ViewBag.UserName = "Login";
            }

            ShareStoryModel viewModel = new ShareStoryModel()
            {
                missionListByUser = await _story.GetMissionsByUser(Convert.ToInt64(UserId))
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveStory([FromForm] IFormCollection form)
        {
            // Access form data using the form parameter
            long missionId = Convert.ToInt64(form["missionId"]);
            String storyTitle = form["storyTitle"];
            String story = form["story"];
            var images = form["images"];
            List<IFormFile> imagesList = new List<IFormFile>();

            // pending
            foreach (var image in images)
            {
                byte[] bytes = Convert.ToString(image.ToString());
                MemoryStream ms = new MemoryStream(bytes);
                IFormFile file = new FormFile(ms, 0, ms.Length, null, Path.GetFileName(ms.ToString()));
                imagesList.Add(file);
            }


            //List<IFormFile> imagesList = form.Files.GetFiles("images").ToList();
            //List<IFormFile> images = form.Files["images[]"].Select(file => (IFormFile)file).ToList();
            //List<IFormFile> images = form.Files.GetFiles("images").Select(file => (IFormFile)file).ToList();

            //List<IFormFile> files = new List<IFormFile>();
            //foreach (byteArray in images)
            //{
            //    MemoryStream stream = new MemoryStream(byteArray);
            //    IFormFile file = new FormFile(stream, 0, stream.Length, "name", "fileName");
            //    files.Add(file);
            //}
            //List<IFormFile> images = form.Files.GetFiles("images").ToList();
            //var images = form.Files.GetFile("images");
            //var imageArray = new IFormFile[images.Length];

            //// Copy the images to an array
            //for (int i = 0; i < images.Length; i++)
            //{
            //    imageArray[i] = images[i];
            //}

            var UserId = "";
            if (HttpContext.Session.GetString("UserName") != null)
            {
                UserId = HttpContext.Session.GetString("UserId");
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
                ViewBag.UserId = UserId;
            }
            else
            {
                ViewBag.UserName = "Login";
            }
            ShareStoryModel viewModel = new ShareStoryModel()
            {
                missionListByUser = await _story.GetMissionsByUser(Convert.ToInt64(UserId))
            };

            Story storyDetails = new Story()
            {
                UserId = Convert.ToInt64(UserId),
                MissionId = missionId,
                Title = storyTitle,
                Description = story,
                Status = "DRAFT"
            };

            // Save the uploaded images in the wwwroot folder
            //if (/*images != null && */images.Count > 0)
            //{
            //    // Get the web root path of the application
            //    string webRootPath = _webHostEnvironment.WebRootPath;

            //    // Create a new folder for the uploaded images
            //    string imagesFolder = Path.Combine(webRootPath, "images");
            //    if (!Directory.Exists(imagesFolder))
            //    {
            //        Directory.CreateDirectory(imagesFolder);
            //    }

            //    // Save each uploaded image to the images folder
            //    foreach (var file in images)
            //    {
            //        // Generate a unique file name for the image
            //        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            //        // Save the image to the images folder
            //        string filePath = Path.Combine(imagesFolder, fileName);
            //        using (var fileStream = new FileStream(filePath, FileMode.Create))
            //        {
            //            await file.CopyToAsync(fileStream);
            //        }

            //        // Add the image details to the StoryMedia table
            //        StoryMedium media = new StoryMedium()
            //        {
            //            StoryId = storyDetails.StoryId,
            //            Type = "image",
            //            Path = "/images/" + fileName
            //        };
            //        _context.StoryMedia.Add(media);
            //    }
            //}

            //// Save the uploaded images in the wwwroot folder
            //if (images != null && images.Length > 0)
            //{
            //    // Get the web root path of the application
            //    string webRootPath = _webHostEnvironment.WebRootPath;

            //    // Create a new folder for the uploaded images
            //    string imagesFolder = Path.Combine(webRootPath, "images");
            //    if (!Directory.Exists(imagesFolder))
            //    {
            //        Directory.CreateDirectory(imagesFolder);
            //    }

            //    Save each uploaded image to the images folder
            //    foreach (IFormFile image in images)
            //    {
            //        // Generate a unique file name for the image
            //        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            //        // Save the image to the images folder
            //        string filePath = Path.Combine(imagesFolder, fileName);
            //        using (var fileStream = new FileStream(filePath, FileMode.Create))
            //        {
            //            await image.CopyToAsync(fileStream);
            //        }

            //        // Add the image details to the StoryMedia table
            //        StoryMedium media = new StoryMedium()
            //        {
            //            StoryId = storyDetails.StoryId,
            //            Type = "image",
            //            Path = "/images/" + fileName
            //        };
            //        _context.StoryMedia.Add(media);
            //    }
            //}

            // Add the story to the database and save changes
            _story.Add(storyDetails);
             _story.Save();

            return View("ShareStory", viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> SubmitStory(ShareStoryModel model)
        {
            var UserId = "";
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
                UserId = HttpContext.Session.GetString("UserId");
                ViewBag.UserId = UserId;
            }
            else
            {
                ViewBag.UserName = "Login";
            }

            return View();
        }
    }
}
