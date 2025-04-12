using My_steam_server.Interfaces;
using My_steam_server.Repositories;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


var filePath = config["JsonRepository:UserFilePath"];

builder.Services.AddSingleton<IUserRepository, JsonUserRepository>(provider=> new JsonUserRepository(filePath));
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();


app.MapControllers();

app.Run();
