using Mekroth.BankApp.Core.Repositories;
using Mekroth.BankApp.Domains.Entities;
using Mekroth.BankApp.Infrastruction.Presistance;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Mekroth.BankApp.Infrastructure.Presistance.Repositories;

public class BankAccountRepository(ILogger<BankAccountRepository> logger, MongoDbContext context) : IBankAccountRepository
{
	private readonly ILogger<BankAccountRepository> _logger = logger;
	private readonly MongoDbContext _context = context;

	public async Task<IEnumerable<BankAccount>> GetAllBankAccounts()
	{
		try
		{
			var bankAccounts = await _context.BankAccounts.Find(_ => true).ToListAsync();
			return bankAccounts;
		}
		catch (Exception ex)
		{
			_logger.LogError("Could not load the bank accounts", ex);
			return null;
		}
	}

	public async Task<BankAccount> GetBankAccount(string accountId)
	{
		try
		{
			var filter = Builders<BankAccount>.Filter.Eq(ba => ba.Id, accountId);
			var bankAccount = await _context.BankAccounts.Find(filter).ToListAsync();
			return bankAccount.FirstOrDefault();
		}
		catch (Exception ex)
		{
			_logger.LogError("Could not load the bank account", ex);
			return null;
		}
	}

	public async Task<bool> AddTransaction(string id, int amount)
	{
		try
		{
			var bankAccount = await GetBankAccount(id);

			if (bankAccount == null)
			{
				await _context.BankAccounts.InsertOneAsync(new() { Id = id, TotalBalance = amount, Transactions = [amount] });
				return true;
			}

			var filter = Builders<BankAccount>.Filter.Eq(ba => ba.Id, id);

			var updateDocument = CreateUpdateDocument(amount);

			await _context.BankAccounts.UpdateOneAsync(filter, updateDocument);
			return true;
		}
		catch (Exception ex)
		{
			_logger.LogError("A error occurred when trying to insert or update bank account", ex);
			return false;
		}
	}

	public async Task<int> GetBalance(string id)
	{
		var bankAccount = await GetBankAccount(id);

		if (bankAccount is null)
		{
			return 0;
		}

		return bankAccount.TotalBalance;
	}

	public async Task<int> GetTotalBalance()
	{
		var totalBalanceKey = nameof(BankAccount.TotalBalance);
		var aggregationQuery = new BsonDocument
		{
			{ "$group", new BsonDocument
				{
					{ "_id", BsonNull.Value },
					{ totalBalanceKey, new BsonDocument { { "$sum", $"${totalBalanceKey}" } } }
				}
			}
		};

		var pipeline = PipelineDefinition<BankAccount, BankAccount>.Create(aggregationQuery);
		var result = await _context.BankAccounts.AsQueryable().SumAsync(b => b.TotalBalance);
		return result;
	}

	private PipelineDefinition<BankAccount, BankAccount> CreateUpdateDocument(int amount)
	{
		var transastionKey = nameof(BankAccount.Transactions);
		var totalBalanceKey = nameof(BankAccount.TotalBalance);
		return PipelineDefinition<BankAccount, BankAccount>.Create(
			new[]
			{
				new BsonDocument
				{
					{ "$set", new BsonDocument
						{
							{ transastionKey, new BsonDocument
								{
									{
										"$concatArrays",
										new BsonArray
										{
											new BsonDocument { { "$ifNull", new BsonArray { $"${transastionKey}", new BsonArray() } } },
											new BsonArray { amount }
										}
									}
								}
							}
						}
					}
				},
				new BsonDocument
				{
					{ "$set", new BsonDocument
						{
							{totalBalanceKey, new BsonDocument { { "$sum", $"${transastionKey}" } }
						}
					}
					}
				}
			}
		);
	}
}
