using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.Api.Models;

public class Device
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required(ErrorMessage = "Device name is required")]
    [StringLength(50, MinimumLength = 2)]
    public string name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Device ID is required")]
    public int deviceId { get; set; }

    public string manufacturer { get; set; } = string.Empty;

    public string type { get; set; } = string.Empty;
    
    public string os { get; set; } = string.Empty;
    
    public string osVersion { get; set; } = string.Empty;
    
    public string processor { get; set; } = string.Empty;
    
    public string ramAmount { get; set; } = string.Empty;

    public string description { get; set; } = string.Empty;

    public int? userId { get; set; } 

    public string generatedDescription { get; set; } = string.Empty;
}