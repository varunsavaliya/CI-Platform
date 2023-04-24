using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CI_Platform_web.Controllers
{
    public class StoryController : Controller
    {
        private readonly IFilters _filters;
        private readonly IStory _story;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StoryController(IFilters filters, IStory story, IWebHostEnvironment webHostEnvironment)
        {
            _filters = filters;
            _story = story;
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
            StoriesListingModel viewModel = new()
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
            var (storyList, totalRecords) = _story.GetStoryCards(inputData);
            StoriesListingModel model = new()
            {
                StoriesList = storyList,
                totalrecords = totalRecords
            };
               
            return PartialView("_StoryPartial", model);
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

            ShareStoryModel viewModel = new()
            {
                missionListByUser = await _story.GetMissionsByUser(Convert.ToInt64(UserId))
            };
            return View(viewModel);
        }

        public IActionResult GetStory(long missionId)
        {
            ViewBag.userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            long UserId = ViewBag.userId;
            if (_story.isStoryAvailable(Convert.ToInt64(UserId), missionId))
            {
                Story availableStory = _story.AvailableStory(Convert.ToInt64(UserId), missionId);
                if (availableStory.Status != "DRAFT")
                {
                    // story is already published or pending
                    return Ok(new { icon = "error", message = "You have already added story for this mission" });
                }
                else
                {
                    return Json(availableStory, new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            }
            return Json(null);
        }

        [HttpPost]
        public async Task<IActionResult> ShareStory(ShareStoryModel model)
        {
            var UserId = "";
            if (HttpContext.Session.GetString("UserName") != null)
            {
                UserId = HttpContext.Session.GetString("UserId");
                ViewBag.UserId = UserId;
            }
            ShareStoryModel viewModel = new()
            {
                missionListByUser = await _story.GetMissionsByUser(Convert.ToInt64(UserId))
            };

            long missionId = model.selectMission;
            String storyTitle = model.storyTitle;
            String story = model.Story;

            Story storyDetails = new()
            {
                UserId = Convert.ToInt64(UserId),
                MissionId = missionId,
                Title = storyTitle,
                Description = story,
                Status = "DRAFT"
            };

            // model.button checks that which button is clicked, save or submit
            if (model.button == 1)
            {
                if (_story.isStoryAvailable(Convert.ToInt64(UserId), missionId))
                {
                    Story availableStory = _story.AvailableStory(Convert.ToInt64(UserId), missionId);
                    if (availableStory.Status != "DRAFT")
                    {
                        // story is already published or pending
                        return Ok(new { icon = "success", message = "You have already added story for this mission" });
                    }
                    else
                    {
                        await _story.UpdateStory(model, availableStory);
                        return Ok(new { icon = "success", message = "Story details updated successfully" });
                    }
                }
                else
                {
                    await _story.AddStoryAsDraft(model, storyDetails);
                    return Ok(new { icon = "success", message = "Story saved successfully" });
                }
            }
            else if(model.button == 2)
            {
                if (_story.isStoryAvailable(Convert.ToInt64(UserId), missionId))
                {
                    Story availableStory = _story.AvailableStory(Convert.ToInt64(UserId), missionId);
                    if (availableStory.Status == "DRAFT")
                    {
                        // story is already published or pending
                        await _story.AddStoryAsPending(model, availableStory);
                        return Ok(new { icon = "success", message = "Story added successfully" });  
                    }
                }
                else
                {
                    return Ok(new { icon = "warning", message = "Story needs to be saved first!!" });
                }
            }
            return View("ShareStory", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> StoryDetail(int id)
        {
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

            StoryDetailModel viewModel = new();
            if (UserId == "")
            {
                viewModel.UserList = null;
            }
            else
            {
                viewModel.UserList = _story.GetUsers(Convert.ToInt64(UserId));
            }
            viewModel.StoryDetail = await _story.GetStoryById(id);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> StoryInvite(long ToUserId, long Id, long FromUserId, StoryDetailModel viewmodel)
        {
            var storyLink = Url.Action("StoryDetail", "Story", new { id = Id }, Request.Scheme);
            await _story.SendEmailInvite(ToUserId, Id, FromUserId, storyLink, viewmodel);
            return Json(new { success = true });
        }
    }
}
