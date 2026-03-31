using DeviceManagement.Api.Models;
using MongoDB.Driver;

namespace DeviceManagement.Api.Services;

public class UserService
{
    private readonly IMongoCollection<User> _usersCollection;
    private readonly SequenceService _sequence;

    public UserService(IConfiguration config, SequenceService sequence)
    {
        _sequence = sequence;
        var client = new MongoClient(config.GetSection("DatabaseSettings:ConnectionString").Value);
        var database = client.GetDatabase(config.GetSection("DatabaseSettings:DatabaseName").Value);
        _usersCollection = database.GetCollection<User>(config.GetSection("DatabaseSettings:UsersCollectionName").Value);
    }

    public async Task<List<User>> GetAsync() => 
        await _usersCollection.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(int id) =>
        await _usersCollection.Find(x => x.userId == id).FirstOrDefaultAsync();

    public async Task<User> CreateAsync(User newUser)
    {
        newUser.userId = await _sequence.GetNextSequenceAsync("userId");
        await _usersCollection.InsertOneAsync(newUser);
        return newUser;
    }

    public async Task UpdateAsync(int id, User updatedUser) =>
        await _usersCollection.ReplaceOneAsync(x => x.userId == id, updatedUser);

    public async Task RemoveAsync(int id) =>
        await _usersCollection.DeleteOneAsync(x => x.userId == id);
}