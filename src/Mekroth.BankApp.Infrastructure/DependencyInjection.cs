using Mekroth.BankApp.Core.Repositories;
using Mekroth.BankApp.Infrastruction.Presistance;
using Mekroth.BankApp.Infrastructure.Presistance.Mappings;
using Mekroth.BankApp.Infrastructure.Presistance.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mekroth.BankApp.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		BankAccountMappings.RegisterClassMap();

		return services.AddSingleton(provider =>
			{
				var connectionString = configuration.GetSection("MongoDb")["ConnectionString"]!;
				var databaseName = configuration.GetSection("MongoDb")["DatabaseName"]!;
				return new MongoDbContext(connectionString, databaseName);
			}
		)
		.AddScoped<IBankAccountRepository, BankAccountRepository>();
	}
}
