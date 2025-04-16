using Mekroth.BankApp.Core.Models;

namespace Mekroth.BankApp.Core.Serivices;

public interface IBankAccountService
{
	Task<int> GetTotalBalance();
	Task<Result<int>> AddTransaction(string accountName, string amount);
}
