using System.Text.Json.Serialization;

namespace MonkeyFinderHybrid.Model;

public class Monkey
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Population { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

// Make sure that this is outside of the Monkey class
[JsonSerializable(typeof(List<Monkey>))]
internal sealed partial class MonkeyContext : JsonSerializerContext
{
    // This class is used to generate the JSON serialization context for the Monkey class.
    // It allows for efficient serialization and deserialization of lists of Monkey objects.
    // It is used in MonkeyService.cs to deserialize the JSON response from the API.
}