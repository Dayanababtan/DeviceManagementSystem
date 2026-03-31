using DeviceManagement.Api.Models;
using MongoDB.Driver;

namespace DeviceManagement.Api.Services;

public class DeviceService
{
    private readonly IMongoCollection<Device> _devicesCollection;
    private readonly SequenceService _sequence;

    public DeviceService(IConfiguration config, SequenceService sequence)
    {
        _sequence = sequence;
        var client = new MongoClient(config["DatabaseSettings:ConnectionString"]);
        var database = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);
        _devicesCollection = database.GetCollection<Device>(config["DatabaseSettings:DevicesCollectionName"]);
    }

    public async Task<List<Device>> GetAsync() => 
        await _devicesCollection.Find(_ => true).ToListAsync();

    public async Task<Device?> GetAsync(int id) => 
        await _devicesCollection.Find(x => x.deviceId == id).FirstOrDefaultAsync();

    public async Task<Device> CreateAsync(Device device)
    {
        device.deviceId = await _sequence.GetNextSequenceAsync("deviceId");
        await _devicesCollection.InsertOneAsync(device);
        return device;
    }

    public async Task UpdateAsync(int id, Device device) => 
        await _devicesCollection.ReplaceOneAsync(x => x.deviceId == id, device);

    public async Task RemoveAsync(int id) => 
        await _devicesCollection.DeleteOneAsync(x => x.deviceId == id);
}