﻿using Ci_Platform.Repositories.Repositories;
using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface IStory
    {
        public Task<List<Story>> GetStories(List<long> storyIds);
    }
}