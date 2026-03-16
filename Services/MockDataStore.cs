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
    public List<Notification> Notifications { get; } = new();
    public List<Gender> Genders { get; } = new();
    public List<City> Cities { get; } = new();

    private int _nextCategoryId = 1;
    private int _nextSkillId = 1;
    private int _nextUserId = 1;
    private int _nextRequestId = 1;
    private int _nextSessionId = 1;
    private int _nextFavoriteId = 1;
    private int _nextUserSkillId = 1;
    private int _nextNotificationId = 1;

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
    public int NextNotificationId() => _nextNotificationId++;

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
    public User AddUserByRegistration(string email, string password, string name, DateOnly? dateOfBirth, int? genderId, int? cityId, string? avatarUrl, string? about, List<int>? learningSkillIds)
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
            CityId = cityId,
            Age = age,
            GenderId = genderId ?? 1,
            DateOfBirth = dateOfBirth,
            AvatarUrl = avatarUrl,
            About = about,
            TeachingSkillIds = new List<int>(),
            LearningSkillIds = learningSkillIds ?? new List<int>()
        };
        Users.Add(u);
        return u;
    }

    private void SeedData()
    {
        // Справочник полов
        Genders.AddRange(new[]
        {
            new Gender { Id = 1, Name = "Не указан" },
            new Gender { Id = 2, Name = "Мужской" },
            new Gender { Id = 3, Name = "Женский" }
        });

        // Справочник городов
        Cities.AddRange(new[]
        {
            new City { Id = 1, Name = "Москва" },
            new City { Id = 2, Name = "Санкт-Петербург" },
            new City { Id = 3, Name = "Новосибирск" },
            new City { Id = 4, Name = "Екатеринбург" },
            new City { Id = 5, Name = "Казань" },
            new City { Id = 6, Name = "Нижний Новгород" },
            new City { Id = 7, Name = "Челябинск" },
            new City { Id = 8, Name = "Самара" },
            new City { Id = 9, Name = "Омск" },
            new City { Id = 10, Name = "Ростов-на-Дону" },
            new City { Id = 11, Name = "Уфа" },
            new City { Id = 12, Name = "Красноярск" },
            new City { Id = 13, Name = "Воронеж" },
            new City { Id = 14, Name = "Пермь" },
            new City { Id = 15, Name = "Волгоград" },
            new City { Id = 16, Name = "Краснодар" },
            new City { Id = 17, Name = "Саратов" },
            new City { Id = 18, Name = "Тюмень" },
            new City { Id = 19, Name = "Тольятти" },
            new City { Id = 20, Name = "Ижевск" },
            new City { Id = 21, Name = "Барнаул" },
            new City { Id = 22, Name = "Иркутск" },
            new City { Id = 23, Name = "Хабаровск" },
            new City { Id = 24, Name = "Ярославль" },
            new City { Id = 25, Name = "Владивосток" },
            new City { Id = 26, Name = "Махачкала" },
            new City { Id = 27, Name = "Томск" },
            new City { Id = 28, Name = "Оренбург" },
            new City { Id = 29, Name = "Кемерово" },
            new City { Id = 30, Name = "Новокузнецк" },
            new City { Id = 31, Name = "Рязань" },
            new City { Id = 32, Name = "Астрахань" },
            new City { Id = 33, Name = "Набережные Челны" },
            new City { Id = 34, Name = "Пенза" },
            new City { Id = 35, Name = "Калининград" },
            new City { Id = 36, Name = "Липецк" },
            new City { Id = 37, Name = "Тула" },
            new City { Id = 38, Name = "Сочи" },
            new City { Id = 39, Name = "Владикавказ" },
            new City { Id = 40, Name = "Архангельск" }
        });

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

        // Пользователи (canTeach 1–5, wantsToLearn 1–5 навыков)
        var u1 = AddUser("Иван", 34, 2, 2, new[] { sMusic.Id, sActing.Id, sCreativeWriting.Id }, new[] { sTimeMgmt.Id, sYogaMeditation.Id, sSpanish.Id, sMindfulness.Id }, avatarUrl: "/Users/Ivan_34_spb.jpg", email: "ivan@mail.ru", password: "test", about: "Привет! Люблю ритм, кофе по утрам и людей, которые не боятся пробовать новое");   // СПб, Мужской
        var u2 = AddUser("Анна", 26, 5, 3, new[] { sEnglish.Id, sEntrepreneurship.Id, sResume.Id, sPersonalBrand.Id, sMarketing.Id }, new[] { sYogaMeditation.Id, sNutrition.Id, sMindfulness.Id }, avatarUrl: "/Users/Anna_26_kaz.jpg", email: "anna@mail.ru", password: "test", about: "Работаю в стартапах, обожаю языки и новые проекты. Ищу баланс через практики и готова делиться опытом.");   // Казань, Женский
        var u3 = AddUser("Максим", 23, 1, 2, new[] { sPhoto.Id, sVideoEdit.Id, sDecor.Id }, new[] { sSpanish.Id, sYogaMeditation.Id, sEnglish.Id, sFitness.Id }, avatarUrl: "/Users/Maksim_23_msk.jpg", email: "maksim@mail.ru", password: "test", about: "Фотограф и путешественник. Мечтаю свободно говорить по-испански и найти свой дзен через практику.");  // Москва, Мужской
        var u4 = AddUser("Елена", 30, 3, 3, new[] { sCooking.Id, sYogaMeditation.Id, sPlants.Id, sNutrition.Id }, new[] { sEnglish.Id, sPhoto.Id, sPersonalDev.Id }, avatarUrl: "/Users/Elena_28_krasn.jpg", email: "elena@mail.ru", password: "test", about: "Готовлю с душой и практикую йогу. Хочу подтянуть английский для путешествий и новых знакомств.");  // Новосибирск, Женский
        var u5 = AddUser("Дмитрий", 28, 4, 2, new[] { sTimeMgmt.Id, sProjectMgmt.Id, sCoaching.Id }, new[] { sMusic.Id, sDrawing.Id, sSleep.Id, sWorkLifeBalance.Id }, avatarUrl: "/Users/Dmitry_28_msk.jpg", email: "dmitry@mail.ru", password: "test", about: "Помогаю другим планировать время и достигать целей. Сам мечтаю наконец научиться играть на гитаре.");  // Екатеринбург, Мужской
        var u6 = AddUser("Ольга", 25, 1, 3, new[] { sYogaMeditation.Id, sMindfulness.Id, sMentalHealth.Id, sFitness.Id }, new[] { sEntrepreneurship.Id, sPhoto.Id, sMarketing.Id, sEnglish.Id }, avatarUrl: "/Users/Olga_27_spb.jpg", email: "olga@mail.ru", password: "test", about: "Йога и осознанность — мой фундамент. Хочу развивать свой проект и научиться красиво его снимать.");  // Москва, Женский

        // Новые пользователи (аватары из wwwroot/Users)
        var u7 = AddUser("Кирилл", 24, 1, 2, new[] { sPhoto.Id, sTimeMgmt.Id, sSpeedReading.Id }, new[] { sEnglish.Id, sVideoEdit.Id, sSales.Id }, avatarUrl: "/Users/Kirill_24_msk.jpg", email: "kirill@mail.ru", password: "test", about: "Снимаю людей и события. Хочу лучше планировать день и уверенно говорить по-английски.");           // Москва, Мужской
        var u8 = AddUser("Наталия", 27, 2, 3, new[] { sSpanish.Id, sCooking.Id, sDrawing.Id, sStorage.Id }, new[] { sYogaMeditation.Id, sPhoto.Id, sEnglish.Id, sNutrition.Id, sMindfulness.Id }, avatarUrl: "/Users/Natalia_27_spb.jpg", email: "natalia@mail.ru", password: "test", about: "Учу испанский и люблю готовить. Хочу углубить практику йоги и научиться классно фотографировать.");   // СПб, Женский
        var u9 = AddUser("Григорий", 30, 5, 2, new[] { sEntrepreneurship.Id, sSales.Id, sTeamMgmt.Id, sProjectMgmt.Id }, new[] { sEnglish.Id, sTimeMgmt.Id, sCoaching.Id }, avatarUrl: "/Users/Grigory_30_kaz.jpg", email: "grigory@mail.ru", password: "test", about: "Запускал свои проекты. Делюсь опытом и сам учусь эффективнее распоряжаться временем и английским.");   // Казань, Мужской
        var u10 = AddUser("Татьяна", 25, 4, 3, new[] { sYogaMeditation.Id, sNutrition.Id, sFitness.Id, sSleep.Id, sWorkLifeBalance.Id }, new[] { sCooking.Id, sDrawing.Id }, avatarUrl: "/Users/Tatyana_25_ekb.jpg", email: "tatyana@mail.ru", password: "test", about: "ЗОЖ и йога — мой образ жизни. Мечтаю готовить так же вкусно и с любовью, как мама.");   // Екатеринбург, Женский
        var u11 = AddUser("Денис", 32, 3, 2, new[] { sMusic.Id, sProjectMgmt.Id, sTeaching.Id, sCognitive.Id }, new[] { sSpanish.Id, sGerman.Id, sVideoEdit.Id }, avatarUrl: "/Users/Denis_32_novosib.jpg", email: "denis@mail.ru", password: "test", about: "Играю в группе и веду проекты. Хочу выучить испанский для поездок в Латинскую Америку.");   // Новосибирск, Мужской
        var u12 = AddUser("Ирина", 23, 38, 3, new[] { sCooking.Id, sDecor.Id, sPlants.Id }, new[] { sPhoto.Id, sEnglish.Id, sVideoEdit.Id, sPersonalBrand.Id }, avatarUrl: "/Users/Irina_23_sochi.jpg", email: "irina@mail.ru", password: "test", about: "Готовлю с настроением, особенно выпечку. Хочу красиво снимать еду и свободно говорить по-английски.");   // Сочи, Женский
        var u13 = AddUser("Богдан", 28, 14, 2, new[] { sTimeMgmt.Id, sCoaching.Id, sPersonalDev.Id, sLearningSkills.Id }, new[] { sMusic.Id, sJapanese.Id }, avatarUrl: "/Users/Bogdan_28_perm.jpg", email: "bogdan@mail.ru", password: "test", about: "Коуч и фанат тайм-менеджмента. Мечтаю научиться играть на инструменте — пока только слушатель.");   // Пермь, Мужской
        var u14 = AddUser("Станислав", 26, 22, 2, new[] { sEnglish.Id, sExamPrep.Id, sLearningSkills.Id }, new[] { sEntrepreneurship.Id, sYogaMeditation.Id, sMarketing.Id, sResume.Id }, avatarUrl: "/Users/Stanislav_26_irk.jpg", email: "stanislav@mail.ru", password: "test", about: "Преподаю английский. Хочу свой бизнес и больше спокойствия — поэтому интересуюсь йогой.");   // Иркутск, Мужской
        var u15 = AddUser("Маргарита", 29, 24, 3, new[] { sDrawing.Id, sDecor.Id, sCreativeWriting.Id, sArtTherapy.Id }, new[] { sCooking.Id, sNutrition.Id, sPhoto.Id, sMindfulness.Id }, avatarUrl: "/Users/Margarita_29_yar.jpg", email: "margarita@mail.ru", password: "test", about: "Рисую и делаю декор своими руками. Хочу готовить полезнее и разнообразнее.");   // Ярославль, Женский
        var u16 = AddUser("Тимофей", 31, 25, 2, new[] { sMusic.Id, sPhoto.Id, sActing.Id, sChinese.Id }, new[] { sTimeMgmt.Id, sJapanese.Id, sCoaching.Id, sVideoEdit.Id }, avatarUrl: "/Users/Timofey_31_vladiv.jpg", email: "timofey@mail.ru", password: "test", about: "Музыкант и фотограф с Дальнего Востока. Учу японский и ищу способы всё успевать.");   // Владивосток, Мужской
        var u17 = AddUser("Павел", 27, 29, 2, new[] { sVideoEdit.Id, sMarketing.Id, sPersonalBrand.Id, sSales.Id }, new[] { sPhoto.Id, sTimeMgmt.Id, sEnglish.Id, sFitness.Id }, avatarUrl: "/Users/Pavel_27_kem.jpg", email: "pavel@mail.ru", password: "test", about: "Монтаж и маркетинг — моя работа. Хочу круто снимать сам и лучше планировать день.");   // Кемерово, Мужской (11-й новый)

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

        // Уведомления по заявкам (req1: u1->u2 Id=1, req2: u3->u4 Id=2)
        Notifications.Add(new Notification { Id = NextNotificationId(), UserId = u2.Id, Type = "ExchangeOffer", ExchangeRequestId = 1, IsRead = false, CreatedAt = DateTime.UtcNow.AddDays(-2) });
        Notifications.Add(new Notification { Id = NextNotificationId(), UserId = u4.Id, Type = "ExchangeOffer", ExchangeRequestId = 2, IsRead = true, CreatedAt = DateTime.UtcNow.AddDays(-1) });
        Notifications.Add(new Notification { Id = NextNotificationId(), UserId = u3.Id, Type = "ExchangeAccepted", ExchangeRequestId = 2, IsRead = true, CreatedAt = DateTime.UtcNow });

        // Избранное
        Favorites.Add(new Favorite { Id = NextFavoriteId(), UserId = u1.Id, TargetUserId = u2.Id, AddedAt = DateTime.UtcNow.AddDays(-1) });
        Favorites.Add(new Favorite { Id = NextFavoriteId(), UserId = u1.Id, TargetUserId = u4.Id, AddedAt = DateTime.UtcNow });

        // Пул фото в корне wwwroot (минимум 4 на скилл)
        var imagePool = new List<string>
        {
            "/drums_1.jpg", "/photo_skill_1.jpg", "/photo_skill_2.jpg", "/drums-1.jpg", "/drums-2.jpg", "/drums-3.jpg",
            "/yoga-1.jpg", "/yoga-2.jpg", "/yoga-3.jpg", "/yoga-4.jpg", "/foto-1.jpg", "/foto-2.jpg", "/foto-3.jpg", "/foto-4.jpg",
            "/spahish-1.jpg", "/spahish-2.jpg", "/food-1.jpg", "/food-2.jpg", "/business-1.jpg", "/time-1.jpg", "/english-1.jpg"
        };
        List<string> TakeImageUrls(int seed)
        {
            var list = new List<string>(4);
            for (var i = 0; i < 4; i++)
                list.Add(imagePool[(seed + i) % imagePool.Count]);
            return list;
        }

        // Навыки пользователей (название, категория, подкатегория, описание, фото) — минимум 4 фото на скилл
        UserSkills.Add(new UserSkill
        {
            Id = NextUserSkillId(),
            UserId = u1.Id,
            Title = "Игра на ударных",
            CategoryId = catArt.Id,
            SkillId = sMusic.Id,
            Description = "Научу базовой постановке рук и простым ритмам. Индивидуальные занятия.",
            ImageUrls = TakeImageUrls(0)
        });
        UserSkills.Add(new UserSkill
        {
            Id = NextUserSkillId(),
            UserId = u3.Id,
            Title = "Портретная съёмка",
            CategoryId = catArt.Id,
            SkillId = sPhoto.Id,
            Description = "Основы композиции и света для портретов. Выезд на локации.",
            ImageUrls = TakeImageUrls(3)
        });

        // У всех пользователей должны быть скиллы в /api/users/{userId}/skills — добавляем по TeachingSkillIds (по 4 фото на скилл)
        var skillIndex = 0;
        foreach (var user in Users)
        {
            foreach (var skillId in user.TeachingSkillIds)
            {
                if (UserSkills.Any(us => us.UserId == user.Id && us.SkillId == skillId))
                    continue;
                var skill = Skills.FirstOrDefault(s => s.Id == skillId);
                if (skill == null) continue;
                AddUserSkill(user.Id, skill.Name, skill.CategoryId, skillId, null, TakeImageUrls(skillIndex++));
            }
        }
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

    private User AddUser(string name, int age, int cityId, int genderId, int[] teaching, int[] learning, string? avatarUrl = null, string? email = null, string? password = null, DateOnly? dateOfBirth = null, string? about = null)
    {
        var id = NextUserId();
        var u = new User
        {
            Id = id,
            Email = !string.IsNullOrEmpty(email) ? email : $"user{id}@test.local",
            Password = password ?? "test",
            Name = name,
            CityId = cityId,
            Age = age,
            GenderId = genderId,
            DateOfBirth = dateOfBirth,
            AvatarUrl = avatarUrl,
            About = about,
            TeachingSkillIds = teaching.ToList(),
            LearningSkillIds = learning.ToList()
        };
        Users.Add(u);
        return u;
    }
}
