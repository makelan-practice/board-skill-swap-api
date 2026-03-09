using SkillSwap.Api.Models;

namespace SkillSwap.Api.Services;

public class ReferenceService
{
    private readonly MockDataStore _store;

    public ReferenceService(MockDataStore store) => _store = store;

    /// <summary>Справочник полов (для формы регистрации и фильтров).</summary>
    public IEnumerable<Gender> GetGenders() => _store.Genders;

    /// <summary>Справочник городов.</summary>
    public IEnumerable<City> GetCities() => _store.Cities;

    /// <summary>Пол по id.</summary>
    public Gender? GetGenderById(int id) => _store.Genders.FirstOrDefault(g => g.Id == id);

    /// <summary>Город по id.</summary>
    public City? GetCityById(int id) => _store.Cities.FirstOrDefault(c => c.Id == id);
}
