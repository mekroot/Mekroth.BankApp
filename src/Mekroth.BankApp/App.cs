using Mekroth.BankApp.Core.Models;
using Mekroth.BankApp.Core.Serivices;

namespace Mekroth.BankApp;

internal class App(IBankAccountService bankAccountService)
{
	private readonly IBankAccountService _bankAccountService = bankAccountService;

	private string _accountName = string.Empty;
	public void RegisterAccount(string accountName)
	{
		_accountName = accountName;
	}

	public async Task<Result<int>> RegisterTransaction(string amount)
	{
		return await _bankAccountService.AddTransaction(_accountName, amount);
	}

	public async Task<int> GetTotalBalance()
	{
		return await _bankAccountService.GetTotalBalance();
	}
}
