using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class StoriesListingModel
    {
        public List<Country> Country { get; set; } = new List<Country>();

        public List<City> City { get; set; } = new List<City>();

        public List<MissionTheme> Theme { get; set; } = new List<MissionTheme>();

        public List<Skill> Skill { get; set; } = new List<Skill>();

        public int totalrecords { get; set; }
        public List<StoryCard> StoriesList { get; set; } = new List<StoryCard>();
    }

    public class StoryCard
    {
        public Story StoryData { get; set; } = new Story();
        public string? UserName { get; set; } = null!;
        public string? StoryMedia { get; set; } = null!;
        public string? ThemeName { get; set; } = null!;
    }
}
