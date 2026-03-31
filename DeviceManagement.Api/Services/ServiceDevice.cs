using DeviceManagement.Api.Models;
using MongoDB.Driver;

namespace DeviceManagement.Api.Services;

public class DeviceService
{
    private readonly IMongoCollection<Device> _devicesCollection;

    public DeviceService(IConfiguration config)
    {
        var client = new MongoClient(config["DatabaseSettings:ConnectionString"]);
        var database = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);
        _devicesCollection = database.GetCollection<Device>(config["DatabaseSettings:DevicesCollectionName"]);
    }

    public async Task<List<Device>> GetAsync() => 
        await _devicesCollection.Find(_ => true).ToListAsync();

    public async Task<Device?> GetAsync(string id) => 
        await _devicesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Device device) => 
        await _devicesCollection.InsertOneAsync(device);

    public async Task UpdateAsync(string id, Device device) => 
        await _devicesCollection.ReplaceOneAsync(x => x.Id == id, device);

    public async Task RemoveAsync(string id) => 
        await _devicesCollection.DeleteOneAsync(x => x.Id == id);
}