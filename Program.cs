using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Identity;
using WebApplic.Data;
using WebApplic.Models;

namespace WebApplic
{
    public class Program
    {
        public static ConcurrentDictionary<string, bool> LogoutCache = new();

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 1;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.Password.RequireUppercase= false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");               
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            
            app.MapRazorPages();

            app.Use(async (context, next) =>
            {
                if (NeedLogout(context.User?.Identity?.Name))
                {
                    context.Response.Redirect("/User/Logout?returnUrl=/Identity/Account/Login");
                }
                await next(context);
            });

            app.Run();
        }

        static bool NeedLogout(string? username)
        {
            if (username is null)
            {
                return false;
            }
            if (LogoutCache.TryGetValue(username, out bool needLogout))
            {
                if (needLogout)
                {
                    LogoutCache[username] = false;
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}