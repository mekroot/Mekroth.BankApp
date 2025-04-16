using Mekroth.BankApp.Application;
using Mekroth.BankApp.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mekroth.BankApp;
internal static class Startup
{
	public static IServiceProvider ConfigurateServices()
	{
		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			.Build();

		var services = new ServiceCollection()
			.AddLogging(options =>
			{
				options.ClearProviders();
				options.AddConsole();
			})
			.AddSingleton<IConfiguration>(configuration)
			.AddInfrastructure(configuration)
			.AddApplication()
			.AddSingleton<App>();

		return services.BuildServiceProvider();
	}
}
