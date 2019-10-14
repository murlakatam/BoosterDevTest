using Booster.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Booster.BlazorApp.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SerilogConfigure.SetupSerilog();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .Build())
                .ConfigureLogging(logging =>
                {
                    logging.
                        AddFilter(
                            "Microsoft.AspNetCore.SignalR",
                            LogLevel.Debug);
                    logging.
                        AddFilter(
                            "Microsoft.AspNetCore.Http.Connections",
                            LogLevel.Debug);
                })
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) =>
                    {
                        loggerConfiguration.ConfigureSerilogConfigFile(hostingContext.Configuration);
                    }
                )
                .Build();
    }
}
