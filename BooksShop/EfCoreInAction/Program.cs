using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EfCoreInAction
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .UseStartup<Startup>()
                .Build()
                .SetupDevelopmentDatabase();
        }

        public static void ConfigureAppConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder configurationBuilder)
        {
            var env = hostingContext.HostingEnvironment;
            configurationBuilder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}