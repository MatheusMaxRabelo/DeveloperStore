using System.Text.Json.Serialization;

namespace DeveloperStore.Sales.Application.Models;

public class Customer
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("name")]
    public Name Name { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("role")]
    public string Role { get; set; }
}

public class Name
{
    [JsonPropertyName("firstname")]
    public string Firstname { get; set; }

    [JsonPropertyName("lastname")]
    public string Lastname { get; set; }
}