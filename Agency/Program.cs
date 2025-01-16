using Agency.DAL;
using Agency.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(opt=>opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddIdentity<AppUser,IdentityRole>(opt=>
{
    opt.Password.RequireNonAlphanumeric=false;
    opt.Password.RequiredLength = 8;
    opt.User.RequireUniqueEmail=true;

    opt.Lockout.AllowedForNewUsers=true;
    opt.Lockout.MaxFailedAccessAttempts=3;
    opt.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(3);
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();



var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    "admin",
    "{area:exists}/{controller=home}/{action=index}/{id?}"

    );

app.MapControllerRoute(
    "default",
    "{controller=home}/{action=index}/{id?}"
    
    );

app.Run();
