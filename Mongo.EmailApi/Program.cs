
using Microsoft.EntityFrameworkCore;
using Mongo.EmailApi.Data;
using Mongo.EmailApi.Extensions;
using Mongo.EmailApi.Messaging;
using Mongo.EmailApi.NewFolder;

namespace Mongo.EmailApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionString));

            // adding dbcontext as singleton not scopped
            var optionbuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionbuilder.UseSqlServer(ConnectionString);
            builder.Services.AddSingleton(new EmailService (optionbuilder.Options));

            builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
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

            app.UseAuthorization();


            app.MapControllers();

            app.UseAzureServiceBusConsumer(); // impleminted to listen to any new message added by shoppingcart in azure messagebus that will fire and run

            app.Run();
        }
    }
}
