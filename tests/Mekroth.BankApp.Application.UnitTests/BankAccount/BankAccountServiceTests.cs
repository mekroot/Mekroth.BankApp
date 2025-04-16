using FluentAssertions;
using FluentAssertions.Execution;
using Mekroth.BankApp.Application.BankAccount;
using Mekroth.BankApp.Core.Repositories;
using NSubstitute;

namespace Mekroth.BankApp.Application.UnitTests.BankAccount;

public sealed class BankAccountServiceTests
{
	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData(null)]
	public async Task AddTransaction_ShouldReturnFailure_WhenAccountName_IsNullOrEmpty(string emptyOrNull)
	{
		// Arrange
		var sut = new BankAccountService(Substitute.For<IBankAccountRepository>());

		// Act
		var result = await sut.AddTransaction(emptyOrNull, "0");

		// Assert

		using (new AssertionScope())
		{
			result.Success.Should().BeFalse();
			result.Errors.Count().Should().Be(1);
			result.Errors.Should().Contain("Account name can't be null or empty, please try again");
		}
	}

	[Theory]
	[InlineData("invalid123")]
	[InlineData("invalid_name")]
	[InlineData("invalid-1")]
	[InlineData("..")]
	[InlineData(".invalid")]
	[InlineData("invalid..name")]
	[InlineData("invalid!")]
	public async Task AddTransaction_ShouldReturnFailure_WhenAccountName_IsInvalid(string invalidAccountName)
	{
		// Arrange
		var sut = new BankAccountService(Substitute.For<IBankAccountRepository>());

		// Act
		var result = await sut.AddTransaction(invalidAccountName, "0");

		// Assert
		using (new AssertionScope())
		{
			result.Success.Should().BeFalse();
			result.Errors.Count().Should().Be(1);
			result.Errors.Should().Contain("Invalid account name, please try again");
		}
	}

	[Theory]
	[InlineData("123invalid")]
	[InlineData("invalid_amount")]
	public async Task AddTransaction_ShouldReturnFailure_WhenAmount_IsInvalid(string invalidAmountName)
	{
		// Arrange
		var sut = new BankAccountService(Substitute.For<IBankAccountRepository>());

		// Act
		var result = await sut.AddTransaction("testAccount", invalidAmountName);

		// Assert
		using (new AssertionScope())
		{
			result.Success.Should().BeFalse();
			result.Errors.Count().Should().Be(1);
			result.Errors.Should().Contain("Invalid amount, please try again");
		}
	}


	[Fact]
	public async Task AddTransaction_ShouldReturnFailure_WhenCurrentBalanceIsZeroAndAmountIsNegative()
	{
		// Arrange
		var accountName = "testAccount";
		var repository = Substitute.For<IBankAccountRepository>();

		repository.GetBalance(accountName).Returns(0);

		var sut = new BankAccountService(repository);

		// Act
		var result = await sut.AddTransaction("testAccount", $"-100");

		// Assert
		using (new AssertionScope())
		{
			result.Success.Should().BeFalse();
			result.Errors.Count().Should().Be(1);
			result.Errors.Should().Contain("Insufficient funds, please try again");

		}
	}

	[Fact]
	public async Task AddTransaction_ShouldReturnSuccess_WhenAmountAndAccountName_IsValid()
	{
		// Arrange
		var accountName = "testAccount";
		var expectedBalance = 100;
		var repository = Substitute.For<IBankAccountRepository>();

		repository.GetBalance(accountName).Returns(100);
		repository.AddTransaction(accountName, 100).Returns(true);

		var sut = new BankAccountService(repository);

		// Act
		var result = await sut.AddTransaction("testAccount", $"{expectedBalance}");

		// Assert
		using (new AssertionScope())
		{
			result.Success.Should().BeTrue();
			result.Errors.Count().Should().Be(0);
			result.Value.Should().Be(expectedBalance);
		}
	}
}