using Microsoft.EntityFrameworkCore;
using Microservices.Data;
using Microsoft.Extensions.Configuration;
using Microservices.API.Consumers;
using Microservices.API.Configurations;
using System.Reflection;
using Microservices.Application;

namespace Microservices.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adiciona configuração do RabbitMQ.Client.
            builder.Services.AddHostedService<BaseProcessMessageConsumer>();
            builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfig"));
            
            Assembly services_assembly = typeof(BaseService<>).GetTypeInfo().Assembly;
            var service_types = services_assembly.ExportedTypes
                .Where(t => t.Name.EndsWith("Service") && !t.Name.StartsWith("Base") && !t.IsInterface)
                .Select(t => new
                {
                    Implementacao = t,
                    Interfaces = t.GetInterfaces()
                }).ToList();
            foreach (var relation in service_types)
            {
                foreach (var Interface in relation.Interfaces)
                {
                    builder.Services.AddScoped(Interface, relation.Implementacao);
                }
            }

            // Adiciona contexto de banco de dados com configuração de conexão do SQL Server
            builder.Services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("cnString"))
                );

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