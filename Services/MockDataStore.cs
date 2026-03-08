using SkillSwap.Api.Models;

namespace SkillSwap.Api.Services;

public class MockDataStore
{
    public List<Category> Categories { get; } = new();
    public List<Skill> Skills { get; } = new();
    public List<User> Users { get; } = new();
    public List<ExchangeRequest> ExchangeRequests { get; } = new();
    public List<ExchangeSession> ExchangeSessions { get; } = new();
    public List<Favorite> Favorites { get; } = new();

    private int _nextCategoryId = 1;
    private int _nextSkillId = 1;
    private int _nextUserId = 1;
    private int _nextRequestId = 1;
    private int _nextSessionId = 1;
    private int _nextFavoriteId = 1;

    public MockDataStore()
    {
        SeedData();
    }

    public int NextCategoryId() => _nextCategoryId++;
    public int NextSkillId() => _nextSkillId++;
    public int NextUserId() => _nextUserId++;
    public int NextRequestId() => _nextRequestId++;
    public int NextSessionId() => _nextSessionId++;
    public int NextFavoriteId() => _nextFavoriteId++;

    private void SeedData()
    {
        // Категории (как на макете)
        var catBiz = AddCategory("Бизнес и карьера");
        var catArt = AddCategory("Творчество и искусство");
        var catLang = AddCategory("Иностранные языки");
        var catEdu = AddCategory("Образование и развитие");
        var catHealth = AddCategory("Здоровье и лайфстайл");
        var catHome = AddCategory("Дом и уют");

        // Навыки
        var sDrum = AddSkill("Игра на барабанах", catArt.Id);
        var sEnglish = AddSkill("Английский язык", catLang.Id);
        var sBusiness = AddSkill("Бизнес-план", catBiz.Id);
        var sTimeMgmt = AddSkill("Тайм-менеджмент", catEdu.Id);
        var sMeditation = AddSkill("Медитация", catHealth.Id);
        var sCooking = AddSkill("Готовка", catHome.Id);
        var sPhoto = AddSkill("Фотография", catArt.Id);
        var sSpanish = AddSkill("Испанский язык", catLang.Id);
        var sYoga = AddSkill("Йога", catHealth.Id);

        // Пользователи
        var u1 = AddUser("Иван", "Санкт-Петербург", 34, "Мужской", new[] { sDrum.Id }, new[] { sTimeMgmt.Id, sMeditation.Id });
        var u2 = AddUser("Анна", "Казань", 26, "Женский", new[] { sEnglish.Id, sBusiness.Id }, new[] { sMeditation.Id });
        var u3 = AddUser("Максим", "Москва", 23, "Мужской", new[] { sPhoto.Id }, new[] { sSpanish.Id, sYoga.Id });
        var u4 = AddUser("Елена", "Новосибирск", 30, "Женский", new[] { sCooking.Id, sYoga.Id }, new[] { sEnglish.Id });
        var u5 = AddUser("Дмитрий", "Екатеринбург", 28, "Мужской", new[] { sTimeMgmt.Id }, new[] { sDrum.Id });
        var u6 = AddUser("Ольга", "Москва", 25, "Женский", new[] { sMeditation.Id }, new[] { sBusiness.Id, sPhoto.Id });

        // Заявки на обмен
        ExchangeRequests.Add(new ExchangeRequest
        {
            Id = NextRequestId(),
            FromUserId = u1.Id,
            ToUserId = u2.Id,
            OfferedSkillId = sDrum.Id,
            RequestedSkillId = sEnglish.Id,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow.AddDays(-2)
        });
        ExchangeRequests.Add(new ExchangeRequest
        {
            Id = NextRequestId(),
            FromUserId = u3.Id,
            ToUserId = u4.Id,
            OfferedSkillId = sPhoto.Id,
            RequestedSkillId = sCooking.Id,
            Status = "Accepted",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            RespondedAt = DateTime.UtcNow
        });

        // Избранное
        Favorites.Add(new Favorite { Id = NextFavoriteId(), UserId = u1.Id, TargetUserId = u2.Id, AddedAt = DateTime.UtcNow.AddDays(-1) });
        Favorites.Add(new Favorite { Id = NextFavoriteId(), UserId = u1.Id, TargetUserId = u4.Id, AddedAt = DateTime.UtcNow });
    }

    private Category AddCategory(string name)
    {
        var c = new Category { Id = NextCategoryId(), Name = name };
        Categories.Add(c);
        return c;
    }

    private Skill AddSkill(string name, int categoryId)
    {
        var s = new Skill { Id = NextSkillId(), Name = name, CategoryId = categoryId };
        Skills.Add(s);
        return s;
    }

    private User AddUser(string name, string city, int age, string gender, int[] teaching, int[] learning)
    {
        var u = new User
        {
            Id = NextUserId(),
            Name = name,
            City = city,
            Age = age,
            Gender = gender,
            TeachingSkillIds = teaching.ToList(),
            LearningSkillIds = learning.ToList()
        };
        Users.Add(u);
        return u;
    }
}
