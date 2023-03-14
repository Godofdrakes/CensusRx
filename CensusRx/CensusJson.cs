using System.Text.Json;
using System.Text.Json.Serialization;
using JorgeSerrano.Json;

namespace CensusRx;

public static class CensusJson
{
	public static JsonSerializerOptions SerializerOptions => new()
	{
		NumberHandling = JsonNumberHandling.AllowReadingFromString,
		PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy(),
	};

	public static IEnumerable<JsonElement> UnwrapCensusCollection(this string json)
	{
		using var document = JsonDocument.Parse(json);
		
		// The property names for a census collection change depending on what was requested
		// but the format is always { <collection>: [array], count: [number] }
		// Just grab the first array property and iterate over the elements within.

		var elements = document.RootElement.EnumerateObject()
			.First(prop => prop.Value.ValueKind == JsonValueKind.Array)
			.Value.EnumerateArray();

		foreach (var element in elements)
		{
			yield return element;
		}
	}

	public static IEnumerable<T> UnwrapCensusCollection<T>(this string json) =>
		json.UnwrapCensusCollection()
			.Select(element => element.Deserialize<T>(SerializerOptions))
			.Select(value => value ?? throw new InvalidOperationException("value is null"));
}
