using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.Api.Models;

public class Device
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required(ErrorMessage = "Device name is required")]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int DeviceID { get; set; }

    public string Manufacturer { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
    
    public string OS { get; set; } = string.Empty;
    
    public string OSVersion { get; set; } = string.Empty;
    
    public string Processor { get; set; } = string.Empty;
    
    public string RAMAmount { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int? UserId { get; set; } 
}