# SkillSwap API (Mock-сервер)

Веб-API на C# (ASP.NET Core 8) для учебного проекта **SkillSwap** — обмен навыками между пользователями. Работает как мок-сервер: данные хранятся в памяти, база данных не используется.

## Запуск локально

```bash
dotnet run
```

После запуска:
- API: **http://localhost:5287**
- Swagger UI: **http://localhost:5287/swagger**

## Docker

Сборка образа:

```bash
docker build -t skillswap-api:latest .
```

Запуск контейнера:

```bash
docker run -d -p 8080:80 --name skillswap-api skillswap-api:latest
```

Или через docker-compose:

```bash
docker-compose up -d
```

API будет доступен на **http://localhost:8080**, Swagger — **http://localhost:8080/swagger**.

## Деплой на Linux-сервер

1. Установите Docker и Docker Compose на сервере.
2. Склонируйте репозиторий или скопируйте файлы проекта (включая `Dockerfile`, `docker-compose.yml`, `wwwroot` с аватарами).
3. В каталоге проекта выполните:
   ```bash
   docker-compose up -d --build
   ```
4. Порт 8080 будет слушать API. Чтобы слушать на 80 или за nginx, измените в `docker-compose.yml` порты на `"80:80"` или настройте reverse proxy.
5. Для доступа снаружи откройте порт в файрволе (например, `ufw allow 8080`).

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
| GET | `/api/users` | Список с фильтрами: `activityType`, `skillIds`, `genderId`, `cityId`, `search` |
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
