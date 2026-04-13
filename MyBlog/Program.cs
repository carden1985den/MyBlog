using AutoMapper;
using BLL.MappingProfile;
using BLL.Services;
using Core;
using Core.Interfaces;
using Core.Interfaces.Services;
using DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add to configuration built-in JSON
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json");

// Get from  JSON configuration SQL Coonection string
var connection = builder.Configuration.GetConnectionString("Connection");

// Add DbContext to the container with SQL Server provider and enable sensitive data logging
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection)
     .EnableSensitiveDataLogging(true));

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/Error/AcceessDenied";
    }
    );

// Register the UnitOfWork as a singleton service
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register services for dependency injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ITagService, TagService>();

// Authorization
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add AutoMapper and register the mapping profile
builder.Services.AddAutoMapper(opition => opition.AddProfile<MappingProfile>());

// NLOG
var logger = LogManager.Setup().LoadConfigurationFromFile($"{Directory.GetCurrentDirectory()}\\nlog.config").GetCurrentClassLogger();
builder.Logging.ClearProviders();
builder.Host.UseNLog();

logger.Debug("Инициализация Приложения");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("Error/SomthingWrong");
    app.UseHsts();
}
app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();

app.UseHttpMethodOverride();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "ResourceNotFound",
    pattern: "{*url}",
    defaults: new { controller = "Error", action = "ResourceNotFound" }
    );

//app.MapGet("/", (ApplicationDbContext dbContext) => dbContext.Users.ToList());

app.Run();
