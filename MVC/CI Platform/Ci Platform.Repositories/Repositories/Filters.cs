﻿using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Repositories
{
    public class Filters : IFilters
    {
        public ApplicationDbContext _context;

        public Filters(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Country>> GetCountriesAsync()
        {
            var country = await _context.Countries.ToListAsync();
            return country;
        }

        public async Task<List<MissionTheme>> GetThemesAsync()
        {
            var theme = await _context.MissionThemes.ToListAsync();
            return theme;
        }
        public async Task<List<Skill>> GetSkillsAsyc()
        {
            var skill = await _context.Skills.ToListAsync();
            return skill;
        }

        public async Task<List<City>> GetCitiesByCountryAsync(int countryId)
        {
            var cities = await _context.Cities.Where(c => c.CountryId == countryId).ToListAsync();
            return cities;
        }
    }
}