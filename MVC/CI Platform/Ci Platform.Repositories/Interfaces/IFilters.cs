using CI_Platform.Entities.DataModels;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface IFilters
    {
        public Task<List<Country>> GetCountriesAsync();
        public Task<List<MissionTheme>> GetThemesAsync();
        public Task<List<Skill>> GetSkillsAsyc();

        public Task<List<City>> GetCitiesByCountryAsync(int countryId);

    }
}
