using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using App.Data;
using App.Extend;
using App.Models;
using App.Service;
using App.Services;
using MailKit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App
{
    public class Startup
    {
        public static string ContentRootPath;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            ContentRootPath = env.ContentRootPath;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            var mailSettings = Configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailSettings);
            services.AddSingleton<IEmailSender,SendMailService>();

            services.AddControllersWithViews();
            services.AddSingleton<ProductService>();
            services.AddSingleton<PlanetService>();
            services.AddDbContext<AppDbContext>(options =>
            {
                string connectString = Configuration.GetConnectionString("MyBlogContext");
                options.UseSqlServer(connectString);
            });
            

            // Đăng ký các dịch vụ của Identity
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Truy cập IdentityOptions
            services.Configure<IdentityOptions>(options =>
            {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true; // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = true; // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false; // Xác thực số điện thoại

            });
            services.AddAuthentication()
                    .AddGoogle(options => {
                        var gconfig = Configuration.GetSection("Authentication:Google");
                        options.ClientId = gconfig["ClientId"];
                        options.ClientSecret = gconfig["ClientSecret"];
                        // https://localhost:5001/signin-google
                        options.CallbackPath =  "/dang-nhap-tu-google";
                    })
                    // .AddTwitter()
                    // .AddMicrosoftAccount()
                    ;
                services.AddSingleton<IdentityErrorDescriber,AppIdentityErrorDescriber>();
                // quyền là admin thì mới hiện lên partial "manage"
                services.AddAuthorization(options =>{
                    options.AddPolicy("ViewManageMenu", builder => {
                        builder.RequireAuthenticatedUser();
                        builder.RequireRole(RoleName.Administrator);
                    });
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.AddStatusCode(); // tuy bien cho cac loi tu 400-599
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                   name: "default",
                   areaName: "ProductManage",
                   pattern: "/{controller}/{action=Index}/{id?}"
               );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
        //dotnet aspnet-codegenerator controller -name  Contact -namespace App.Areas.Contact.Controllers -m App.Models.Contact -udl -dc App.Models.AppDbContext -outDir Areas/Contact/Controller
    }
}
