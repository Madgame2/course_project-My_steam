using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using My_steam_server.Interfaces;
using My_steam_server.Models;
using My_steam_server.Repositories;
using My_steam_server.Services;
using System.Text;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Загрузка конфигурации
    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

    var config = builder.Configuration;


    var filePath = config["JsonRepository:UserFilePath"];
    var TokenFilepath = config["JsonRepository:TokenFilePath"];
    var GoodsFilepath = config["JsonRepository:GoodsFilePath"];

    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddSingleton<IUserRepository, JsonUserRepository>(provider => new JsonUserRepository(filePath));
    builder.Services.AddSingleton<IRefreshTokenRepository, JsonRefreshTokenRepository>(provider => new JsonRefreshTokenRepository(TokenFilepath));
    builder.Services.AddSingleton<IGoodRepository<Game>>(provider => new JsonGoodsRepository<Game>(GoodsFilepath));
    builder.Services.AddControllers();

    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IGoodsService<Game>>(provider => new GoodsService<Game>(provider.GetRequiredService<IGoodRepository<Game>>()));
    builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Authentication failed: " + context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });

    // Добавление CORS для работы с фронтендом
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    // Использование CORS
    app.UseCors("AllowAll");

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine("Сервер упал с ошибкой:");
    Console.WriteLine(ex.ToString());
}