using SkillSwap.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SkillSwap API", Version = "v1" });
    var xmlPath = Path.Combine(AppContext.BaseDirectory, "SkillSwap.Api.xml");
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

// Мок-сервер: один экземпляр хранилища в памяти
builder.Services.AddSingleton<MockDataStore>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<SkillService>();
builder.Services.AddScoped<ReferenceService>();
builder.Services.AddScoped<UserSkillService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ExchangeRequestService>();
builder.Services.AddScoped<ExchangeSessionService>();
builder.Services.AddScoped<FavoriteService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<MatchService>();

// CORS для фронтенда
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// API и Swagger доступны и в Production (для документации на сервере)
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseStaticFiles(); // статические файлы из wwwroot (аватары: /Users/...)
app.MapControllers();

app.Run();
