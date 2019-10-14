using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Booster.Configuration
{
    public static class SerilogConfigure
    {
        public static ILogger CreateSerilogLogger(this IConfiguration config)
        {
            Log.Logger = new LoggerConfiguration().ConfigureSerilogConfigFile(config).CreateLogger();
            var loggerFactory = new LoggerFactory().AddSerilog(Log.Logger);
            return loggerFactory.CreateLogger("Default");
        }

        public static LoggerConfiguration ConfigureSerilogConfigFile(this LoggerConfiguration loggerConfig, IConfiguration config)
        {
            return loggerConfig.ReadFrom.Configuration(config);
        }

        public static void SetupSerilog()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            Environment.SetEnvironmentVariable("SERILOG_BASEDIR", basePath);

#if DEBUG
            Serilog.Debugging.SelfLog.Enable(msg => System.Console.WriteLine(msg));
#endif
        }

    }

    
}
