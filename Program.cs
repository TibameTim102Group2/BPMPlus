using BPMPlus.Data;
using BPMPlus.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Configuration;

namespace BPMPlus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // �qappsettings.jsonŪ���n�J�O�ɳ]�w
            double LoginExpireMinute = builder.Configuration.GetValue<double>("LoginExpireMinute");

            // �إ����Ҥ����n��A��
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                // �ثe�O���ɶ��]�w��60���� (appsettings.json)
                option.ExpireTimeSpan = TimeSpan.FromMinutes(LoginExpireMinute);
                // ����cookie���ੵ��
                option.SlidingExpiration = false;
                option.LoginPath = "/Login/Index";
            });

            // �]�w MVC �P CSRF ����
            builder.Services.AddControllersWithViews(options => {
                // CSRF��w�����A�o�̴N�[�J�������ҽd��Filter���ܡA�ݷ|Controller�N�����A�[�W[AutoValidateAntiforgeryToken]�ݩ�
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
            

            app.Run();
        }
    }
}
