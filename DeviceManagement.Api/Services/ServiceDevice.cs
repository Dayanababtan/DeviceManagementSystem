using DeviceManagement.Api.Models;
using MongoDB.Driver;

namespace DeviceManagement.Api.Services;

public class DeviceService
{
    private readonly IMongoCollection<Device> _devicesCollection;
    private readonly SequenceService _sequence;
    private readonly AIService _aiService;

    public DeviceService(IConfiguration config, SequenceService sequence, AIService aiService)
    {
        _sequence = sequence;
        _aiService = aiService;
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
        try {
            device.generatedDescription = await _aiService.GenerateDescriptionAsync(device);
        } catch {
            device.generatedDescription = "A reliable device from " + device.manufacturer;
        }
        device.deviceId = await _sequence.GetNextSequenceAsync("deviceId");
        await _devicesCollection.InsertOneAsync(device);
        return device;
    }

    public async Task UpdateAsync(int id, Device device)
    {
        var existingDevice = await GetAsync(id);
        if (existingDevice is null)
        {
            throw new InvalidOperationException($"Device with id {id} not found.");
        }

        device.Id = existingDevice.Id;
        device.deviceId = existingDevice.deviceId;

        await _devicesCollection.ReplaceOneAsync(x => x.deviceId == id, device);
    }

    public async Task RemoveAsync(int id) => 
        await _devicesCollection.DeleteOneAsync(x => x.deviceId == id);
}