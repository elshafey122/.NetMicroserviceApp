using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Mongo.MessageBus;
using Mongo.ShoppingCartApi;
using Mongo.ShoppingCartApi.Data;
using Mongo.ShoppingCartApi.Extentions;
using Mongo.ShoppingCartApi.Services.Implemintations;
using Mongo.ShoppingCartApi.Services.Interfaces;
using Mongo.ShoppingCartApi.Utility;

namespace Mongo.ProductApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Add services to the container.

            var productApi = builder.Configuration["ServicesUrl:ProductApi"];
            var couponApi = builder.Configuration["ServicesUrl:CouponApi"];

            SD.ProductApi = productApi;
            SD.CouponApi = couponApi;


            var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionString));

            builder.Services.AddScoped<Services.Interfaces.IShoppingCartRepository, Services.Implemintations.ShoppingCartRepository>();
            builder.Services.AddScoped<IRestRepository, RestRepository>();
            builder.Services.AddScoped<IMessageBus, MessagesBus>();
            builder.Services.AddAutoMapper(typeof(MappingConfig));

            #region swagger generator for authorization
            // it is used to enable authorization on swagger
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference= new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id=JwtBearerDefaults.AuthenticationScheme
                            }
                        }, new string[]{}
                    }
                });
            });
            #endregion

            #region auth
            // add auth and authori
            builder.AddAppAuthetication(); // setting for jwt 
            builder.Services.AddAuthorization();
            #endregion


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            ApplyMigration();

            app.Run();


            void ApplyMigration()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var Db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    if (Db.Database.GetPendingMigrations().Count() > 0)
                    {
                        Db.Database.Migrate();
                    }
                }
            }
        }
    }
}
