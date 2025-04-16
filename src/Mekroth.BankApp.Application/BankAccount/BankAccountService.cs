using Mekroth.BankApp.Core.Models;
using Mekroth.BankApp.Core.Repositories;
using Mekroth.BankApp.Core.Serivices;
using System.Text.RegularExpressions;

namespace Mekroth.BankApp.Application.BankAccount
{
	public class BankAccountService(IBankAccountRepository bankAccountRepository) : IBankAccountService
	{
		// Make sure the account name following best practice
		// adam.nilson and adamNilson is OK
		// adam_nilson is not OK
		private static readonly Regex accountNameRegex = new Regex(
		@"^([A-Za-z]+)(\.[A-Za-z]+)*$",
		RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private readonly IBankAccountRepository _bankAccountRepository = bankAccountRepository;

		public Task<int> GetTotalBalance()
		{
			return _bankAccountRepository.GetTotalBalance();
		}

		public async Task<Result<int>> AddTransaction(string accountName, string amount)
		{
			if (!IsValidAccountName(accountName, out var errors))
			{
				return errors;
			}

			var validateResult = await CheckValidAmount(accountName, amount);

			if (validateResult is not null && !validateResult.Success)
			{
				return validateResult;
			}

			var result = await _bankAccountRepository.AddTransaction(accountName, validateResult!.Value).ConfigureAwait(false);

			if (!result)
			{
				return new Result<int>(["Something when wrong, please try again"], 0);
			}

			var newBalance = await _bankAccountRepository.GetBalance(accountName).ConfigureAwait(false);

			return new Result<int>([], newBalance);
		}

		private bool IsValidAccountName(string accountName, out Result<int> errors)
		{
			errors = null;

			if (string.IsNullOrEmpty(accountName?.Trim()))
			{
				errors = new Result<int>(["Account name can't be null or empty, please try again"], 0);
				return false;
			}

			if (!accountNameRegex.IsMatch(accountName))
			{
				errors = new Result<int>(["Invalid account name, please try again"], 0);
				return false;
			}

			return true;
		}


		private async Task<Result<int>> CheckValidAmount(string accountName, string amount)
		{
			if (!int.TryParse(amount, out var parsedAmount))
			{
				return new Result<int>(["Invalid amount, please try again"], 0);
			}

			var currentBalance = await _bankAccountRepository.GetBalance(accountName).ConfigureAwait(false);

			if (currentBalance == 0 && parsedAmount < 0)
			{
				return new Result<int>(["Insufficient funds, please try again"], 0);
			}

			return new Result<int>([], parsedAmount);
		}
	}
}
