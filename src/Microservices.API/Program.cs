using Microsoft.EntityFrameworkCore;
using Microservices.Data;
using Microsoft.Extensions.Configuration;
using Microservices.API.Consumers;
using Microservices.API.Configurations;

namespace Microservices.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHostedService<BaseProcessMessageConsumer>();
            builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfig"));
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("cnString")));
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

            app.Run();
        }
    }
}