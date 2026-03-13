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
- **Уведомления** — предложения обмена и принятие обмена (новые / просмотренные, прочитать все, очистить).
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

### Уведомления `GET/PATCH/POST/DELETE /api/notifications`
| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/notifications?userId=&unreadOnly=false` | Список уведомлений (unreadOnly — только новые) |
| PATCH | `/api/notifications/{id}/read?userId=` | Отметить одно как прочитанное |
| POST | `/api/notifications/mark-all-read?userId=` | Прочитать все |
| DELETE | `/api/notifications/clear-viewed?userId=` | Очистить просмотренные |

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
- Уведомления пользователя 2: `GET http://localhost:5287/api/notifications?userId=2`
- Только новые уведомления: `GET http://localhost:5287/api/notifications?userId=2&unreadOnly=true`

Данные при старте заполняются тестовыми пользователями, навыками, заявками и избранным (см. `Services/MockDataStore.cs`).

---

## Инструкция: API уведомлений (`/api/notifications`)

Уведомления привязаны к заявкам на обмен: «вам предложили обмен» и «ваше предложение приняли». По ним можно получить список, отметить прочитанными или очистить просмотренные.

### Параметры и поля ответа

**Параметры запроса (GET список):**
- `userId` (обязательный) — id пользователя, чьи уведомления запрашиваются.
- `unreadOnly` (необязательный, по умолчанию `false`) — если `true`, возвращаются только непрочитанные (новые).

**Поля объекта уведомления в ответе (NotificationDto):**

| Поле | Тип | Описание |
|------|-----|----------|
| `id` | int | Идентификатор уведомления |
| `type` | string | Тип: `ExchangeOffer` (вам предложили обмен) или `ExchangeAccepted` (ваше предложение приняли) |
| `message` | string | Заголовок, например: «Иван предлагает вам обмен» или «Анна приняла ваш обмен» |
| `subMessage` | string | Подсказка: «Примите обмен, чтобы обсудить детали» или «Перейдите в профиль, чтобы обсудить детали» |
| `exchangeRequestId` | int | Id заявки на обмен (для перехода к экрану заявки) |
| `relatedUserId` | int | Id пользователя, с которым связано уведомление (для перехода в профиль) |
| `relatedUserName` | string | Имя этого пользователя |
| `isRead` | bool | Прочитано (`true`) или новое (`false`) |
| `createdAt` | string (ISO 8601) | Дата и время создания уведомления (UTC) |

Список возвращается отсортированным по `createdAt` по убыванию (сначала новые).

### Типы уведомлений

| type | Когда создаётся | Для кого |
|------|-----------------|----------|
| `ExchangeOffer` | Кто-то отправил вам заявку на обмен | Получатель заявки |
| `ExchangeAccepted` | Получатель принял вашу заявку на обмен | Инициатор заявки |

### Пошаговый пример (локальный сервер на порту 5287)

Базовый URL: `http://localhost:5287`. В тестовых данных у пользователя с `userId=2` (Анна) есть одно непрочитанное уведомление.

**Шаг 1. Получить все уведомления пользователя 2**
```http
GET http://localhost:5287/api/notifications?userId=2
```
Ответ (пример): массив с одним элементом — «Иван предлагает вам обмен», `isRead: false`, `exchangeRequestId: 1`, `relatedUserId: 1`, `relatedUserName: "Иван"`.

**Шаг 2. Получить только новые (непрочитанные) уведомления**
```http
GET http://localhost:5287/api/notifications?userId=2&unreadOnly=true
```
В ответе — только уведомления с `isRead: false`.

**Шаг 3. Отметить одно уведомление как прочитанное**

Допустим, id уведомления = 1. Тогда:
```http
PATCH http://localhost:5287/api/notifications/1/read?userId=2
```
Ответ: `204 No Content`. После этого при запросе списка у этого уведомления будет `isRead: true`.

**Шаг 4. Прочитать все (отметить все как прочитанные)**
```http
POST http://localhost:5287/api/notifications/mark-all-read?userId=2
```
Ответ (пример): `200 OK`, тело `{ "markedCount": 1 }` — количество отмеченных уведомлений.

**Шаг 5. Очистить просмотренные (удалить прочитанные из списка)**
```http
DELETE http://localhost:5287/api/notifications/clear-viewed?userId=2
```
Ответ (пример): `200 OK`, тело `{ "removedCount": 1 }`. После этого при `GET ...?userId=2` прочитанные уведомления больше не вернутся.

### Использование на экране уведомлений

- **Блок «Новые уведомления»:** `GET /api/notifications?userId={id}&unreadOnly=true`. Кнопка «Прочитать все» → `POST /api/notifications/mark-all-read?userId={id}`.
- **Блок «Просмотренные»:** `GET /api/notifications?userId={id}` (без `unreadOnly`), отображать только элементы с `isRead: true`. Кнопка «Очистить» → `DELETE /api/notifications/clear-viewed?userId={id}`.
- **Кнопка «Перейти»:** по `exchangeRequestId` открыть экран заявки (`GET /api/exchangerequests/{id}`) или по `relatedUserId` — профиль пользователя (`GET /api/users/{id}`).

---

## Хостинг (nv.142932)

- **Тариф:** lin.ubuntu2404v1, NV 1.96 (Москва, РТКомм/ТрастИнфо)
- **IP сервера:** `81.176.228.41` (маска /23)
- **API и Swagger на сервере:** после деплоя — `http://81.176.228.41:8080` и `http://81.176.228.41:8080/swagger`

### Добавление IP адреса

Если панель хостинга или фаервол просят «добавить IP» — укажите туда адрес вашего сервера `81.176.228.41` (или диапазон, если указан /23). Для доступа к API с интернета порт **8080** должен быть открыт; остальные порты на тарифе открыты, 25-й порт закрыт.

### Порт 25 закрыт — как отправлять почту

На многих VPS порт 25 закрыт для борьбы со спамом. Варианты:

1. **SMTP по портам 587 или 465** — используйте «Submission» (587) или SMTPS (465) у вашего почтового провайдера (Яндекс, Mail.ru, Gmail, Mailgun, SendGrid и т.д.). В приложении настройте SMTP-клиент на хост и порт из инструкции провайдера (обычно 587 или 465), не 25.
2. **Внешний API** — SendGrid, Mailgun, Amazon SES, Yandex Cloud и др. отправка через их HTTP API, порт 25 не нужен.
3. **Ретранслятор хостинга** — если у провайера есть SMTP relay (часто на 587), используйте его.

В текущем проекте отправка писем не реализована; при добавлении — достаточно указать хост:порт 587 или 465 и учётные данные в конфиге.

### Web VNC (консоль)

- Адрес: **http://81.177.160.11:10428** (или из панели хостинга).
- Откройте ссылку в браузере — отобразится VNC-консоль сервера (логин в систему при необходимости через учётку сервера).

### Web SSH

- В панели хостинга есть вход по Web SSH (браузерный терминал). Пароль и ссылку смотрите в панели управления тарифом. Альтернатива — обычный SSH с вашего ПК: `ssh root@81.176.228.41`.
