using Mekroth.BankApp.Domains.Entities;

namespace Mekroth.BankApp.Core.Repositories;

public interface IBankAccountRepository
{
	Task<IEnumerable<BankAccount>> GetAllBankAccounts();
	Task<BankAccount> GetBankAccount(string accountId);
	Task<bool> AddTransaction(string accountId, int amount);
	Task<int> GetTotalBalance();
	Task<int> GetBalance(string accountId);
}
