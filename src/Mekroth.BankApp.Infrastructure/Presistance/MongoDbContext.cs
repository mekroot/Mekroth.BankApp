using Mekroth.BankApp.Domains.Entities;
using MongoDB.Driver;

namespace Mekroth.BankApp.Infrastruction.Presistance;

public class MongoDbContext
{
	private readonly IMongoDatabase _database;

	public MongoDbContext(string connectionString, string databaseName)
	{
		var client = new MongoClient(connectionString);
		_database = client.GetDatabase(databaseName);
	}

	public IMongoDatabase Database { get { return _database; } }

	public IMongoCollection<BankAccount> BankAccounts => _database.GetCollection<BankAccount>(nameof(BankAccounts));
}
