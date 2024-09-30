using BPMPlus.Data;
using BPMPlus.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using static BPMPlus.Service.AesAndTimestampService;

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
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<AesAndTimestampService>();
            builder.Services.AddScoped<ResetPasswordService>();


            // 從appsettings.json讀取登入逾時設定
            //double LoginExpireMinute = builder.Configuration.GetValue<double>("LoginExpireMinute");

            // 建立驗證中介軟體服務
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                // 目前逾期時間設定為60分鐘 (appsettings.json)
                option.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                // 啟用cookie滑動過期
                option.SlidingExpiration = true;
                option.LoginPath = "/Login/Index";
            });

            // 設定 MVC 與 CSRF 驗證
            builder.Services.AddControllersWithViews(options => {
                // CSRF資安有關，這裡就加入全域驗證範圍Filter的話，待會Controller就不必再加上[AutoValidateAntiforgeryToken]屬性
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            //讓JSON回傳中文字不是亂碼
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All);
            });

            //// 使用記憶體作為 Session 存儲
            //// 過期時間, cookie透過https連線
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true; 
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            var app = builder.Build();
            //測試自動部屬
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate(); // 自動應用遷移
            }
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

            // 使用session
            app.UseSession();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}
