using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.Api.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; }

    [Required(ErrorMessage = "User ID is required")]
    public int userId { get; set; }

    [Required(ErrorMessage = "User name is required")]
    public string name { get; set; } = string.Empty;

    [Required]
    public string role { get; set; } = string.Empty; 

    [Required]
    public string location { get; set; } = string.Empty;

    public string email { get; set; } = string.Empty;
    
    public string password { get; set; } = string.Empty;
}