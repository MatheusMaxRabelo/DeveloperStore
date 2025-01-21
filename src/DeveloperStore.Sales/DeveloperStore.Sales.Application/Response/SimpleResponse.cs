using System.Text.Json.Serialization;

namespace DeveloperStore.Sales.Application.Response;

public class SimpleResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
}
