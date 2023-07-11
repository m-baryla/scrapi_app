using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using scrapi.Service;
using scrapi.DbAccess;
using scrapi.DtoBase;
using scrapi.Engine;

internal class Program
{
    private static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();

        Serilog.Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

        Serilog.Log.Information("Application start");

        var host = Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureServices((context, services) =>
            {
                services.AddTransient<ISqlDataAccess, SqlDataAccess>();
                services.AddTransient<IDataBaseAcces, DataBaseAcces>();
                services.AddTransient<IScrapEngine, ScrapEngine>();

                services.AddAutoMapper(typeof(Program));
            })
            .Build();

        var svc = ActivatorUtilities.CreateInstance<ScrapService>(host.Services);
        svc.Run();

        Serilog.Log.Information("Application stop");

        Console.ReadKey();
        Environment.Exit(0);
    }
}
