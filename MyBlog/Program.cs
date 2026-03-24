using AutoMapper;
using BLL.Entity;
using BLL.MappingProfile;
using DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
        options.AccessDeniedPath = "/User/AcceessDenied";
    }
    );

// Register the UnitOfWork as a singleton service
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Authorization
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add AutoMapper and register the mapping profile
builder.Services.AddAutoMapper(opition => opition.AddProfile<MappingProfile>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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


//app.MapGet("/", (ApplicationDbContext dbContext) => dbContext.Users.ToList());

app.Run();
