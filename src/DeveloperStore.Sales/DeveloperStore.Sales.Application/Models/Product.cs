using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeveloperStore.Sales.Application.Models;

public class Product
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("image")]
    public string Image { get; set; }

    [JsonPropertyName("rating")]
    public Rating Rating { get; set; }
}

public class Rating
{
    [JsonPropertyName("rate")]
    public double Rate { get; set; }

    [JsonPropertyName("count")]
    public double Count { get; set; }
}
