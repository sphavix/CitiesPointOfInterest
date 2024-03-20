using CityPointOfInterest;
using CityPointOfInterest.DataContext;
using CityPointOfInterest.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day).CreateLogger();

var builder = WebApplication.CreateBuilder(args);
// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();

builder.Host.UseSerilog();
// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddProblemDetails();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

builder.Services.AddSingleton<CitiesDataStore>();
builder.Services.AddDbContext<CityInfoDbContext>(options =>
{
    options.UseSqlite("Data Source=CityInfo.db");
});
//#if DEBUG
builder.Services.AddTransient<IEmailService, EmailService>();
// #else
// builder.Services.AddTransient<IEmailService, CloudMailService>();
// #endif

var app = builder.Build();

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
