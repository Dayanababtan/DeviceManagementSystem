using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace DeviceManagement.Api.Services;

public class SequenceService
{
    private readonly IMongoCollection<Counter> _countersCollection;

    public SequenceService(IConfiguration config)
    {
        var client = new MongoClient(config["DatabaseSettings:ConnectionString"]);
        var database = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);
        _countersCollection = database.GetCollection<Counter>("Counters");
    }

    public async Task<int> GetNextSequenceAsync(string name)
    {
        var filter = Builders<Counter>.Filter.Eq(c => c.Id, name);
        var update = Builders<Counter>.Update.Inc(c => c.Value, 1);
        var options = new FindOneAndUpdateOptions<Counter> { IsUpsert = true, ReturnDocument = ReturnDocument.After };

        var updated = await _countersCollection.FindOneAndUpdateAsync(filter, update, options);
        return updated.Value;
    }
}

public class Counter
{
    [BsonElement("Id")]
    public string Id { get; set; } = string.Empty;
    public int Value { get; set; }
}