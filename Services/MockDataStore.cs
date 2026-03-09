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
    public List<UserSkill> UserSkills { get; } = new();

    private int _nextCategoryId = 1;
    private int _nextSkillId = 1;
    private int _nextUserId = 1;
    private int _nextRequestId = 1;
    private int _nextSessionId = 1;
    private int _nextFavoriteId = 1;
    private int _nextUserSkillId = 1;

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
    public int NextUserSkillId() => _nextUserSkillId++;

    /// <summary>Добавить навык пользователя (название, категория, подкатегория, описание, фото).</summary>
    public UserSkill AddUserSkill(int userId, string title, int categoryId, int skillId, string? description, List<string>? imageUrls)
    {
        var us = new UserSkill
        {
            Id = NextUserSkillId(),
            UserId = userId,
            Title = title,
            CategoryId = categoryId,
            SkillId = skillId,
            Description = description,
            ImageUrls = imageUrls ?? new List<string>()
        };
        UserSkills.Add(us);
        return us;
    }

    /// <summary>Регистрация нового пользователя (для Auth).</summary>
    public User AddUserByRegistration(string email, string password, string name, DateOnly? dateOfBirth, string? gender, string? city, string? avatarUrl, List<int>? learningSkillIds)
    {
        var age = dateOfBirth.HasValue
            ? (int)((DateTime.Today - dateOfBirth.Value.ToDateTime(TimeOnly.MinValue)).TotalDays / 365.25)
            : 0;
        var u = new User
        {
            Id = NextUserId(),
            Email = email.Trim(),
            Password = password,
            Name = name,
            City = city ?? "",
            Age = age,
            Gender = gender ?? "Не указан",
            DateOfBirth = dateOfBirth,
            AvatarUrl = avatarUrl,
            TeachingSkillIds = new List<int>(),
            LearningSkillIds = learningSkillIds ?? new List<int>()
        };
        Users.Add(u);
        return u;
    }

    private void SeedData()
    {
        // Категории (порядок как в списке всех навыков)
        var catBiz = AddCategory("Бизнес и карьера");
        var catLang = AddCategory("Иностранные языки");
        var catHome = AddCategory("Дом и уют");
        var catArt = AddCategory("Творчество и искусство");
        var catEdu = AddCategory("Образование и развитие");
        var catHealth = AddCategory("Здоровье и лайфстайл");

        // Бизнес и карьера
        var sTeamMgmt = AddSkill("Управление командой", catBiz.Id);
        var sMarketing = AddSkill("Маркетинг и реклама", catBiz.Id);
        var sSales = AddSkill("Продажи и переговоры", catBiz.Id);
        var sPersonalBrand = AddSkill("Личный бренд", catBiz.Id);
        var sResume = AddSkill("Резюме и собеседование", catBiz.Id);
        var sTimeMgmt = AddSkill("Тайм-менеджмент", catBiz.Id);
        var sProjectMgmt = AddSkill("Проектное управление", catBiz.Id);
        var sEntrepreneurship = AddSkill("Предпринимательство", catBiz.Id);

        // Иностранные языки
        var sEnglish = AddSkill("Английский", catLang.Id);
        var sFrench = AddSkill("Французский", catLang.Id);
        var sSpanish = AddSkill("Испанский", catLang.Id);
        var sGerman = AddSkill("Немецкий", catLang.Id);
        var sChinese = AddSkill("Китайский", catLang.Id);
        var sJapanese = AddSkill("Японский", catLang.Id);
        var sExamPrep = AddSkill("Подготовка к экзаменам (IELTS, TOEFL)", catLang.Id);

        // Дом и уют
        var sCleaning = AddSkill("Уборка и организация", catHome.Id);
        var sHomeFinance = AddSkill("Домашние финансы", catHome.Id);
        var sCooking = AddSkill("Приготовление еды", catHome.Id);
        var sPlants = AddSkill("Домашние растения", catHome.Id);
        var sRepair = AddSkill("Ремонт", catHome.Id);
        var sStorage = AddSkill("Хранение вещей", catHome.Id);

        // Творчество и искусство
        var sDrawing = AddSkill("Рисование и иллюстрация", catArt.Id);
        var sPhoto = AddSkill("Фотография", catArt.Id);
        var sVideoEdit = AddSkill("Видеомонтаж", catArt.Id);
        var sMusic = AddSkill("Музыка и звук", catArt.Id);
        var sActing = AddSkill("Актёрское мастерство", catArt.Id);
        var sCreativeWriting = AddSkill("Креативное письмо", catArt.Id);
        var sArtTherapy = AddSkill("Арт-терапия", catArt.Id);
        var sDecor = AddSkill("Декор и DIY", catArt.Id);

        // Образование и развитие
        var sPersonalDev = AddSkill("Личностное развитие", catEdu.Id);
        var sLearningSkills = AddSkill("Навыки обучения", catEdu.Id);
        var sCognitive = AddSkill("Когнитивные техники", catEdu.Id);
        var sSpeedReading = AddSkill("Скорочтение", catEdu.Id);
        var sTeaching = AddSkill("Навыки преподавания", catEdu.Id);
        var sCoaching = AddSkill("Коучинг", catEdu.Id);

        // Здоровье и лайфстайл
        var sYogaMeditation = AddSkill("Йога и медитация", catHealth.Id);
        var sNutrition = AddSkill("Питание и ЗОЖ", catHealth.Id);
        var sMentalHealth = AddSkill("Ментальное здоровье", catHealth.Id);
        var sMindfulness = AddSkill("Осознанность", catHealth.Id);
        var sFitness = AddSkill("Физические тренировки", catHealth.Id);
        var sSleep = AddSkill("Сон и восстановление", catHealth.Id);
        var sWorkLifeBalance = AddSkill("Баланс жизни и работы", catHealth.Id);

        // Пользователи (привязаны к навыкам и аватарам из wwwroot/Users)
        var u1 = AddUser("Иван", "Санкт-Петербург", 34, "Мужской", new[] { sMusic.Id }, new[] { sTimeMgmt.Id, sYogaMeditation.Id }, avatarUrl: "/Users/Ivan_34_spb.jpg", email: "ivan@mail.ru", password: "test");
        var u2 = AddUser("Анна", "Казань", 26, "Женский", new[] { sEnglish.Id, sEntrepreneurship.Id }, new[] { sYogaMeditation.Id }, avatarUrl: "/Users/Anna_26_kaz.jpg", email: "anna@mail.ru", password: "test");
        var u3 = AddUser("Максим", "Москва", 23, "Мужской", new[] { sPhoto.Id }, new[] { sSpanish.Id, sYogaMeditation.Id }, avatarUrl: "/Users/Maksim_23_msk.jpg", email: "maksim@mail.ru", password: "test");
        var u4 = AddUser("Елена", "Новосибирск", 30, "Женский", new[] { sCooking.Id, sYogaMeditation.Id }, new[] { sEnglish.Id }, avatarUrl: "/Users/Elena_28_krasn.jpg", email: "elena@mail.ru", password: "test");
        var u5 = AddUser("Дмитрий", "Екатеринбург", 28, "Мужской", new[] { sTimeMgmt.Id }, new[] { sMusic.Id }, avatarUrl: "/Users/Dmitry_28_msk.jpg", email: "dmitry@mail.ru", password: "test");
        var u6 = AddUser("Ольга", "Москва", 25, "Женский", new[] { sYogaMeditation.Id }, new[] { sEntrepreneurship.Id, sPhoto.Id }, avatarUrl: "/Users/Olga_27_spb.jpg", email: "olga@mail.ru", password: "test");

        // Заявки на обмен
        ExchangeRequests.Add(new ExchangeRequest
        {
            Id = NextRequestId(),
            FromUserId = u1.Id,
            ToUserId = u2.Id,
            OfferedSkillId = sMusic.Id,
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

        // Навыки пользователей (название, категория, подкатегория, описание, фото)
        UserSkills.Add(new UserSkill
        {
            Id = NextUserSkillId(),
            UserId = u1.Id,
            Title = "Игра на ударных",
            CategoryId = catArt.Id,
            SkillId = sMusic.Id,
            Description = "Научу базовой постановке рук и простым ритмам. Индивидуальные занятия.",
            ImageUrls = new List<string> { "/Skills/drums_1.jpg" }
        });
        UserSkills.Add(new UserSkill
        {
            Id = NextUserSkillId(),
            UserId = u3.Id,
            Title = "Портретная съёмка",
            CategoryId = catArt.Id,
            SkillId = sPhoto.Id,
            Description = "Основы композиции и света для портретов. Выезд на локации.",
            ImageUrls = new List<string> { "/Skills/photo_skill_1.jpg", "/Skills/photo_skill_2.jpg" }
        });
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

    private User AddUser(string name, string city, int age, string gender, int[] teaching, int[] learning, string? avatarUrl = null, string? email = null, string? password = null, DateOnly? dateOfBirth = null)
    {
        var id = NextUserId();
        var u = new User
        {
            Id = id,
            Email = !string.IsNullOrEmpty(email) ? email : $"user{id}@test.local",
            Password = password ?? "test",
            Name = name,
            City = city,
            Age = age,
            Gender = gender,
            DateOfBirth = dateOfBirth,
            AvatarUrl = avatarUrl,
            TeachingSkillIds = teaching.ToList(),
            LearningSkillIds = learning.ToList()
        };
        Users.Add(u);
        return u;
    }
}
