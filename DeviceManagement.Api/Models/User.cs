using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.Api.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required(ErrorMessage = "User name is required")]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty; // e.g., Admin, Developer, Manager

    [Required]
    public string Location { get; set; } = string.Empty; // e.g., New York, London, Remote
}