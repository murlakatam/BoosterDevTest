using System;
using Microsoft.Extensions.Configuration;

namespace Booster.Configuration
{
    public static class SettingsConfigure
    {
        public static IConfigurationBuilder SetupAppSettingsAndLogging(this IConfigurationBuilder config)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            config
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.logging.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true);
            config.AddEnvironmentVariables();

            return config;
        }

    }

}
