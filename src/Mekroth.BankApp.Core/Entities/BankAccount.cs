namespace Mekroth.BankApp.Domains.Entities;

public sealed class BankAccount
{
	public required string Id { get; set; }
	public int TotalBalance { get; set; }
	public List<int> Transactions { get; set; } = [];
}
