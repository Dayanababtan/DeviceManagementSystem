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
        var id = devices![0].deviceId;

        var response = await _client.GetAsync($"/api/devices/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Device_3_Post_CreatesNewDevice()
    {
        var newDevice = new Device
        {
            name = "Test iPhone",
            manufacturer = "Apple",
            type = "Smartphone",
            ramAmount = "8"
        };

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

        target.manufacturer = "Updated Manufacturer";

        var response = await _client.PutAsJsonAsync($"/api/devices/{target.deviceId}", target);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Device_5_Delete_RemovesDevice()
    {
        var newDevice = new Device { name = "Delete Me", manufacturer = "Test", type = "SmartPhone" };
        var postRes = await _client.PostAsJsonAsync("/api/devices", newDevice);
        var created = await postRes.Content.ReadFromJsonAsync<Device>();

        var response = await _client.DeleteAsync($"/api/devices/{created!.deviceId}");
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
        var id = users![0].userId;

        var response = await _client.GetAsync($"/api/users/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task User_4_Put_UpdatesUser()
    {
        var users = await _client.GetFromJsonAsync<List<User>>("/api/users");
        var target = users![0];
        target.location = "Updated City";

        var response = await _client.PutAsJsonAsync($"/api/users/{target.userId}", target);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task User_5_Delete_RemovesUser()
    {
        var newUser = new User { name = "Disposable", role = "Guest", location = "None", email = "test@delete.com", password = "password" };
        var postRes = await _client.PostAsJsonAsync("/api/auth/register", newUser); // Use auth/register to avoid validation issues

        var users = await _client.GetFromJsonAsync<List<User>>("/api/users");
        var created = users!.Last();
        var response = await _client.DeleteAsync($"/api/users/{created!.userId}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}