﻿using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class LandingPageModel
    {
        public List<Country> Country { get; set; }

        public List<City> City { get; set; }

        public List<MissionTheme> Theme { get; set; }

        public List<Skill> Skill { get; set; }
        public List<MissionSkill> MissionSkills { get; set; }

        public List<Mission> MissionList { get; set; }
    }
}

