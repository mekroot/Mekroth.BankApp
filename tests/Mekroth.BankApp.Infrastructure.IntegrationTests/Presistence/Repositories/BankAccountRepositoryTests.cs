using FluentAssertions;
using Mekroth.BankApp.Infrastructure.Presistance.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Mekroth.BankApp.Infrastructure.IntegrationTests.Presistence.Repositories;

public sealed class BankAccountRepositoryTests : MongoDbContainerTestBase
{
	[Fact]
	public async Task AddTransaction_ShouldSuccessAddingTransaction_WhenAccountNotExist()
	{
		// Arrange
		var logger = Substitute.For<ILogger<BankAccountRepository>>();
		var sut = new BankAccountRepository(logger, Context);
		var noneExistingAccountName = "accountZero";

		// Act
		var notExistingAccount = await sut.GetBankAccount(noneExistingAccountName) == null;
		notExistingAccount.Should().BeTrue();

		var result = await sut.AddTransaction(noneExistingAccountName, 100);
		var account = await sut.GetBankAccount(noneExistingAccountName);
		var balance = await sut.GetBalance(noneExistingAccountName);

		// Assert
		account.Should().NotBeNull();
		balance.Should().Be(account.TotalBalance);
	}

	[Fact]
	public async Task AddTransaction_ShouldSuccessAddingTransaction_WhenAccountExist()
	{
		// Arrange
		var logger = Substitute.For<ILogger<BankAccountRepository>>();
		var sut = new BankAccountRepository(logger, Context);
		var accountName = "accountOne";

		// Act
		await sut.AddTransaction(accountName, 100);
		await sut.AddTransaction(accountName, 200);
		var account = await sut.GetBankAccount(accountName);

		// Assert
		account.TotalBalance.Should().Be(300);
	}

	[Fact]
	public async Task AddTransaction_ShouldHaveBalanceZero_WhenDepositFullBalanceAmount()
	{
		// Arrange
		var logger = Substitute.For<ILogger<BankAccountRepository>>();
		var sut = new BankAccountRepository(logger, Context);
		var accountName = "accountTwo";

		// Act
		await sut.AddTransaction(accountName, 100);
		await sut.AddTransaction(accountName, -100);
		var account = await sut.GetBankAccount(accountName);

		// Assert
		account.TotalBalance.Should().Be(0);
	}

	[Fact]
	public async Task GetTotalBalance_ShouldGetZero_WhenNoTransactionExists()
	{
		// Arrange
		var logger = Substitute.For<ILogger<BankAccountRepository>>();
		var sut = new BankAccountRepository(logger, Context);

		// Act
		var result = await sut.GetTotalBalance();

		// Assert
		result.Should().Be(0);
	}

	[Fact]
	public async Task GetTotalBalance_ShouldGetBankTotalBalance_WhenOneAccountExists()
	{
		// Arrange
		var logger = Substitute.For<ILogger<BankAccountRepository>>();
		var sut = new BankAccountRepository(logger, Context);

		// Act
		var accountName = "accountThree";
		await sut.AddTransaction(accountName, 100);
		await sut.AddTransaction(accountName, 300);
		var account = await sut.GetBankAccount(accountName);

		var result = await sut.GetTotalBalance();

		// Assert
		result.Should().Be(400);
	}


	[Fact]
	public async Task GetTotalBalance_ShouldGetBankTotalBalance_WhenMultipleAccountExists()
	{
		// Arrange
		var logger = Substitute.For<ILogger<BankAccountRepository>>();
		var sut = new BankAccountRepository(logger, Context);

		// Act
		var testOne = "testOne";
		await sut.AddTransaction(testOne, 100);
		await sut.AddTransaction(testOne, 300);

		var testTwo = "testTwo";
		await sut.AddTransaction(testTwo, 100);
		await sut.AddTransaction(testTwo, -300);

		var result = await sut.GetTotalBalance();

		// Assert
		result.Should().Be(200);
	}
}
