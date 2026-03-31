using System.Net;
using System.Net.Http.Json;
using DeviceManagement.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DeviceManagement.Tests;

public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Device_1_GetAll_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/api/devices");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Device_2_GetById_ReturnsDevice()
    {
        var devices = await _client.GetFromJsonAsync<List<Device>>("/api/devices");
        var id = devices![0].Id;

        var response = await _client.GetAsync($"/api/devices/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Device_3_Post_CreatesNewDevice()
    {
        var newDevice = new Device { name = "Test iPhone", manufacturer = "Apple", type = "Phone", ramAmount = "8GB" };
        var response = await _client.PostAsJsonAsync("/api/devices", newDevice);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<Device>();
        Assert.NotNull(created);
        Assert.True(created!.deviceId > 0, "deviceId should be auto-assigned and > 0");
    }

    [Fact]
    public async Task Device_4_Put_UpdatesDevice()
    {
        var devices = await _client.GetFromJsonAsync<List<Device>>("/api/devices");
        var target = devices![0];
        target.Description = "Updated via Test";

        var response = await _client.PutAsJsonAsync($"/api/devices/{target.Id}", target);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Device_5_Delete_RemovesDevice()
    {
        var newDevice = new Device { name = "Delete Me", manufacturer = "Test", type = "Phone" };
        var postRes = await _client.PostAsJsonAsync("/api/devices", newDevice);
        var created = await postRes.Content.ReadFromJsonAsync<Device>();

        var response = await _client.DeleteAsync($"/api/devices/{created!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task User_1_GetAll_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/api/users");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task User_2_GetById_ReturnsUser()
    {
        var users = await _client.GetFromJsonAsync<List<User>>("/api/users");
        var id = users![0].Id;

        var response = await _client.GetAsync($"/api/users/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task User_3_Post_CreatesNewUser()
    {
        var newUser = new User { name = "Test User", role = "Admin", location = "Cluj" };
        var response = await _client.PostAsJsonAsync("/api/users", newUser);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<User>();
        Assert.NotNull(created);
        Assert.True(created!.userId > 0, "userId should be auto-assigned and > 0");
    }

    [Fact]
    public async Task User_4_Put_UpdatesUser()
    {
        var users = await _client.GetFromJsonAsync<List<User>>("/api/users");
        var target = users![0];
        target.location = "Updated City";

        var response = await _client.PutAsJsonAsync($"/api/users/{target.Id}", target);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task User_5_Delete_RemovesUser()
    {
        var newUser = new User { name = "Disposable", role = "Guest", location = "None" };
        var postRes = await _client.PostAsJsonAsync("/api/users", newUser);
        var created = await postRes.Content.ReadFromJsonAsync<User>();

        var response = await _client.DeleteAsync($"/api/users/{created!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}