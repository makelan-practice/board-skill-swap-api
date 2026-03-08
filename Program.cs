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
builder.Services.AddScoped<ExchangeRequestService>();
builder.Services.AddScoped<ExchangeSessionService>();
builder.Services.AddScoped<FavoriteService>();
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapControllers();

app.Run();
