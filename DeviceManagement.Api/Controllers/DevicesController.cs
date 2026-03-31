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
    public async Task<ActionResult<Device>> Get(int id)
    {
        var device = await _deviceService.GetAsync(id);
        if (device is null) return NotFound();
        return device;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Device newDevice)
    {
        await _deviceService.CreateAsync(newDevice);
        return CreatedAtAction(nameof(Get), new { id = newDevice.deviceId }, newDevice);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(int id, Device updatedDevice)
    {
        var device = await _deviceService.GetAsync(id);
        if (device is null) return NotFound();
        updatedDevice.deviceId = device.deviceId;
        await _deviceService.UpdateAsync(id, updatedDevice);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var device = await _deviceService.GetAsync(id);
        if (device is null) return NotFound();
        await _deviceService.RemoveAsync(id);
        return NoContent();
    }
}