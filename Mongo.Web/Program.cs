using Microsoft.AspNetCore.Authentication.Cookies;
using Mongo.Web.Services.Implemintations;
using Mongo.Web.Services.Interfaces;
using Mongo.Web.Utilites;

namespace Mongo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            SD.CouponApi = builder.Configuration["ServiceUrls:CouponApi"];
            SD.AuthApi = builder.Configuration["ServiceUrls:AuthApi"];
            SD.ProductApi = builder.Configuration["ServiceUrls:ProductApi"];
            SD.ShoppingCartApi = builder.Configuration["ServiceUrls:ShoppingCartApi"];


            builder.Services.AddScoped<ICouponRestService, CouponRestService>();
            builder.Services.AddScoped<IAuthRestService, AuthRestService>();
            builder.Services.AddScoped<ITokenProvider, TokenProvider>();
            builder.Services.AddScoped<IPrductRestService, PrductRestService>();
            builder.Services.AddScoped<ICartService, CartService>();


            #region  auth and authori using cookie
            // auth and authori using cookie
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) //add cookies for authentication for client
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(10);
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Auth/AccessDenied";
            });
            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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
