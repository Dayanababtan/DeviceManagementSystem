using MongoDB.Driver;

namespace DeviceManagement.Api.Services;

public class SequenceService
{
    private readonly IMongoCollection<Counter> _countersCollection;

    public SequenceService(IConfiguration config)
    {
        var client = new MongoClient(config["DatabaseSettings:ConnectionString"]);
        var database = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);
        _countersCollection = database.GetCollection<Counter>(config["DatabaseSettings:CountersCollectionName"] ?? "Counters");
    }

    public async Task<int> GetNextSequenceAsync(string name)
    {
        var filter = Builders<Counter>.Filter.Eq(c => c.Id, name);
        var update = Builders<Counter>.Update.Inc(c => c.Value, 1);
        var options = new FindOneAndUpdateOptions<Counter>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        var updated = await _countersCollection.FindOneAndUpdateAsync(filter, update, options);
        if (updated == null)
        {
            var counter = new Counter { Id = name, Value = 1 };
            await _countersCollection.InsertOneAsync(counter);
            return counter.Value;
        }

        return updated.Value;
    }
}

public class Counter
{
    public string Id { get; set; } = string.Empty;
    public int Value { get; set; }
}
