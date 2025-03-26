using Contatos.DeleteConsumer.Interfaces;
using Contatos.DeleteConsumer.Repository;
using Contatos.DeleteConsumer.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MassTransit;
using Serilog;
using Serilog.Events;

namespace Contatos.DeleteConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(path: "Logs/DeleteConsumerLogs.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddSerilog();

                builder.Services.AddAuthorization();
                builder.Services.AddOpenApi();

                builder.Services.AddMassTransit(x =>
                {
                    x.AddConsumer<ApagarContatoConsumer>();

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host("rabbitmq");

                        //cfg.Host("localhost", "/", h =>
                        //{
                        //    h.Username("guest");
                        //    h.Password("guest");
                        //});

                        cfg.ConfigureEndpoints(context);
                    });
                });

                builder.Services.AddSingleton<IContatoRepository, ContatoRepository>();

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                }

                app.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                });
                app.MapHealthChecks("/health/live", new HealthCheckOptions());

                app.MapGet("/", () => Results.Ok("Servi�o online.")).WithName("HealthCheck");

                app.UseHttpsRedirection();
                app.UseAuthorization();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal($"O servi�o Contatos.DeleteConsumer encerrou inesperadamente. Exception: {ex.GetType()}. Message: {ex.Message}.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
