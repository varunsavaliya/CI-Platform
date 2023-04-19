using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using Microsoft.EntityFrameworkCore;

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
            var countries = await _context.Countries.ToListAsync();
            return countries ?? new List<Country>();
        }

        public async Task<List<MissionTheme>> GetThemesAsync()
        {
            var theme = await _context.MissionThemes.Where(theme => theme.DeletedAt == null).ToListAsync();
            return theme;
        }
        public async Task<List<Skill>> GetSkillsAsyc()
        {
            var skill = await _context.Skills.Where(skill => skill.DeletedAt == null).ToListAsync();
            return skill;
        }

        public async Task<List<City>> GetCitiesByCountryAsync(int countryId)
        {
            var cities = await _context.Cities.Where(c => c.CountryId == countryId).ToListAsync();
            return cities;
        }

        public async Task<List<CmsTable>> GetCmsTables()
        {
            return await _context.CmsTables.ToListAsync();
        }
    }
}
