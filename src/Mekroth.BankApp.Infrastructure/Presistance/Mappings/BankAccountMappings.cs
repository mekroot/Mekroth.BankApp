using Mekroth.BankApp.Domains.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Mekroth.BankApp.Infrastructure.Presistance.Mappings;

public static class BankAccountMappings
{
	public static void RegisterClassMap()
	{
		if (!BsonClassMap.IsClassMapRegistered(typeof(BankAccount)))
		{
			BsonClassMap.RegisterClassMap<BankAccount>(entity =>
			{
				entity.AutoMap();
				entity.MapIdMember(e => e.Id)
					.SetSerializer(new StringSerializer(BsonType.String));
			});
		}
	}
}
