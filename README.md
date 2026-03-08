# SkillSwap API (Mock-сервер)

Веб-API на C# (ASP.NET Core 8) для учебного проекта **SkillSwap** — обмен навыками между пользователями. Работает как мок-сервер: данные хранятся в памяти, база данных не используется.

## Запуск

```bash
dotnet run
```

После запуска:
- API: **http://localhost:5287**
- Swagger UI: **http://localhost:5287/swagger**

## Основные сущности

- **Пользователь** — «Учу» (чему может научить) и «Учусь» (чему хочет научиться).
- **Навыки и категории** — список навыков по категориям (бизнес, творчество, языки и т.д.).
- **Заявки на обмен** — создание и принятие/отклонение заявок.
- **Сессии обмена** — активные или завершённые обмены между двумя пользователями.
- **Избранное** — сохранение понравившихся пользователей.
- **Подбор пар** — поиск пользователей, с которыми возможен взаимный обмен навыками.

## Эндпоинты

### Пользователи `GET/POST /api/users`
| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/users` | Список с фильтрами: `activityType`, `skillIds`, `gender`, `city`, `search` |
| GET | `/api/users/{id}` | Карточка пользователя |
| GET | `/api/users/popular?count=6` | Популярные |
| GET | `/api/users/new?count=6` | Новые |
| GET | `/api/users/recommended?userId=1&count=6` | Рекомендуемые для обмена |

### Навыки `GET /api/skills`
| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/skills/categories` | Все категории |
| GET | `/api/skills/categories/{id}` | Категория по id |
| GET | `/api/skills?categoryId=&search=` | Навыки (опционально по категории и поиск) |
| GET | `/api/skills/{id}` | Навык по id |

### Заявки на обмен `GET/POST /api/exchangerequests`
| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/exchangerequests?userId=&status=` | Список заявок |
| GET | `/api/exchangerequests/{id}` | Заявка по id |
| POST | `/api/exchangerequests` | Создать заявку (body: `CreateExchangeRequestDto`) |
| POST | `/api/exchangerequests/{id}/respond?accept=true|false` | Принять/отклонить |

### Сессии обмена `GET/POST /api/exchangesessions`
| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/exchangesessions?userId=&status=` | Список сессий |
| GET | `/api/exchangesessions/{id}` | Сессия по id |
| POST | `/api/exchangesessions/{id}/complete` | Завершить сессию |
| POST | `/api/exchangesessions/{id}/cancel` | Отменить сессию |

### Избранное `GET/POST/DELETE /api/favorites`
| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/favorites?userId=` | Избранные пользователи |
| POST | `/api/favorites?userId=&targetUserId=&targetSkillId=` | Добавить в избранное |
| DELETE | `/api/favorites?userId=&targetUserId=` | Удалить из избранного |
| GET | `/api/favorites/check?userId=&targetUserId=` | Проверить, в избранном ли |

### Подбор пар `GET /api/match`
| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/match?userId=&maxCount=20` | Пользователи для взаимного обмена навыками |

## Примеры запросов

- Карточки пользователей: `GET http://localhost:5287/api/users`
- Поиск по навыку: `GET http://localhost:5287/api/users?search=английский`
- Фильтр «Могу научить»: `GET http://localhost:5287/api/users?activityType=Могу научить`
- Категории: `GET http://localhost:5287/api/skills/categories`
- Подбор пар для пользователя 1: `GET http://localhost:5287/api/match?userId=1`

Данные при старте заполняются тестовыми пользователями, навыками, заявками и избранным (см. `Services/MockDataStore.cs`).
