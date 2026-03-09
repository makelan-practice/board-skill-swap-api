# Сборка
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SkillSwap.Api.csproj", "."]
RUN dotnet restore "SkillSwap.Api.csproj"

COPY . .
RUN dotnet build "SkillSwap.Api.csproj" -c Release -o /app/build

# Публикация
FROM build AS publish
RUN dotnet publish "SkillSwap.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Рантайм
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Пользователь без root (безопасность)
RUN adduser --disabled-password --gecos "" appuser
COPY --from=publish /app/publish .
RUN chown -R appuser:appuser /app
USER appuser

# Порт и URL для контейнера (на сервере можно переопределить)
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "SkillSwap.Api.dll"]
