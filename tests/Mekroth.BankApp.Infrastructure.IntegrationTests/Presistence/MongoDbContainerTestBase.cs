using Mekroth.BankApp.Infrastruction.Presistance;
using Mekroth.BankApp.Infrastructure.Presistance.Mappings;
using Testcontainers.MongoDb;

namespace Mekroth.BankApp.Infrastructure.IntegrationTests.Presistence;

public abstract class MongoDbContainerTestBase : IAsyncLifetime
{
	private readonly MongoDbContainer _mongoDbContainer;
	protected MongoDbContext Context { get; private set; }

	protected MongoDbContainerTestBase()
	{
		_mongoDbContainer = new MongoDbBuilder()
					.WithUsername("testuser")
					.WithPassword("testpass")
					.Build();
	}

	public async Task DisposeAsync()
	{
		await _mongoDbContainer.DisposeAsync();
	}

	public async Task InitializeAsync()
	{
		await _mongoDbContainer.StartAsync();

		BankAccountMappings.RegisterClassMap();

		Context = new MongoDbContext(_mongoDbContainer.GetConnectionString(), "Bank");
		Context.Database.DropCollection(nameof(MongoDbContext.BankAccounts));

		await OnDatabaseInitialized();
	}

	protected virtual Task OnDatabaseInitialized() => Task.CompletedTask;
}
