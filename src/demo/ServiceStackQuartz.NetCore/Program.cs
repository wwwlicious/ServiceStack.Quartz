namespace ServiceStack.Quartz.NetCore
{
    using System;
    using Serilog;
    using Serilog.Events;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                BuildWebHost(args).Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        
        public static IHost BuildWebHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                .Build();
    }
}