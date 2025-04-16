using Mekroth.BankApp.Application.BankAccount;
using Mekroth.BankApp.Core.Serivices;
using Microsoft.Extensions.DependencyInjection;

namespace Mekroth.BankApp.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		return services.AddScoped<IBankAccountService, BankAccountService>();
	}
}
