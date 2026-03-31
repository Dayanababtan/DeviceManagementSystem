using DeviceManagement.Api.Models;
using DeviceManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly DeviceService _deviceService;

    public DevicesController(DeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet]
    public async Task<List<Device>> Get() => await _deviceService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Device>> Get(string id)
    {
        var device = await _deviceService.GetAsync(id);
        if (device is null) return NotFound();
        return device;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Device newDevice)
    {
        await _deviceService.CreateAsync(newDevice);
        return CreatedAtAction(nameof(Get), new { id = newDevice.Id }, newDevice);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Device updatedDevice)
    {
        var device = await _deviceService.GetAsync(id);
        if (device is null) return NotFound();
        updatedDevice.Id = device.Id;
        await _deviceService.UpdateAsync(id, updatedDevice);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var device = await _deviceService.GetAsync(id);
        if (device is null) return NotFound();
        await _deviceService.RemoveAsync(id);
        return NoContent();
    }
}