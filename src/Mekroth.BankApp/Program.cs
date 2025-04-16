using Mekroth.BankApp;
using Microsoft.Extensions.DependencyInjection;

var app = Startup.ConfigurateServices()
				 .GetRequiredService<App>();

var hasExit = false;

Console.WriteLine("Welcome to the bank!");
Console.WriteLine();

while (!hasExit)
{

	Console.Write("Please enter account name [empty to exit]: ");
	var accountName = Console.ReadLine();

	if (string.IsNullOrWhiteSpace(accountName))
	{
		hasExit = true;
		continue;
	}

	app.RegisterAccount(accountName);

	var hasError = false;

	do
	{
		Console.Write("Please enter amount: ");
		var amount = Console.ReadLine();

		var result = await app.RegisterTransaction(amount!);
		if (result.Success)
		{
			Console.WriteLine($"Current balance: {result.Value}");
			hasError = false;
		}
		else
		{
			foreach (var error in result.Errors)
			{
				Console.WriteLine(error);
			}
			hasError = true;
		}
	} while (hasError);
}

var totalBalance = await app.GetTotalBalance();
Console.WriteLine($"Bank total balance: {totalBalance}");
Console.WriteLine();
Console.WriteLine("Please press [Enter] to exit...");
while (Console.ReadKey(true).Key != ConsoleKey.Enter)
{
}