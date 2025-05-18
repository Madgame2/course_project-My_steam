using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using My_steam_server;
using My_steam_server.Interfaces;
using My_steam_server.Models;
using My_steam_server.Repositories;
using My_steam_server.Services;
using System.Text;
using Microsoft.EntityFrameworkCore;
using My_steam_server.Repositories.DB;

var builder = WebApplication.CreateBuilder(args);

    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

    var config = builder.Configuration;


//    var filePath = config["JsonRepository:UserFilePath"];
//    var TokenFilepath = config["JsonRepository:TokenFilePath"];
//    var GoodsFilepath = config["JsonRepository:GoodsFilePath"];
//    var PurchousesFilepath = config["JsonRepository:PurchousesFilePath"];
//    var LibFilepath = config["JsonRepository:LibFilePath"];
//    var ReportsFilepath = config["JsonRepository:ReportsFilePath"];
var Screenpath = config["JsonRepository:ScreeensPath"];


builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 512 * 1024 * 1024; // 512 MB
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 512 * 1024 * 1024; // 512 MB
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IUserRepository, EFUserRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IUserLibraryRepository, EfUserLibraryRepository>();
builder.Services.AddScoped<IReportsRepository, EfReportsRepository>();
builder.Services.AddScoped<IReportsService, ReportService>();
builder.Services.AddScoped<IRefreshTokenRepository, EfRefreshTokenRepository>();
builder.Services.AddScoped<IPurchaseOptionRepository, EfPurchaseOptionRepository>();
builder.Services.AddScoped<IGoodRepository, EfGoodRepository>();
builder.Services.AddScoped<IBoughtService, BoughtService>(provider =>
{
    var UserRep = provider.GetRequiredService<IUserRepository>();
    var PurhcouseOptions = provider.GetRequiredService<IPurchaseOptionRepository>();
    var GoodRepository = provider.GetRequiredService<IGoodRepository>();
    var LibRepos = provider.GetRequiredService<IUserLibraryRepository>();

    return new BoughtService(UserRep, PurhcouseOptions, GoodRepository, LibRepos);
});


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddSingleton<IGamesStaticFilesRepository, GamesStaticFilesRepository>();
    builder.Services.AddSingleton<IResources>(provider => 
    {
        var staticFilesRepository = provider.GetRequiredService<IGamesStaticFilesRepository>();
        return new ResourcesService(staticFilesRepository);
    });
    builder.Services.AddSingleton<IGamesRespository, GamesRepository>();    
   
builder.Services.AddSingleton<IScreenShotsRepository, ScreenShotsRepository>(provider =>
{

    return new ScreenShotsRepository(Screenpath);
});

builder.Services.AddSingleton<IPublisherService, PublisherService>(provider =>
{
    var gameRep = provider.GetRequiredService<IGamesRespository>();
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var staticFiles = provider.GetRequiredService<IGamesStaticFilesRepository>();
    var screenShtots = provider.GetRequiredService<IScreenShotsRepository>();

    return new PublisherService(gameRep, screenShtots, httpContextAccessor, staticFiles);
});
    builder.Services.AddControllers();

    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IGoodsService>(provider => new GoodsService(provider.GetRequiredService<IGoodRepository>()));
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

    // ���������� CORS ��� ������ � ����������
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

    // ������������� CORS
    app.UseCors("AllowAll");

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

