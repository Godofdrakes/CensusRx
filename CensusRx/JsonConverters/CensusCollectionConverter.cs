using System.Text.Json;
using System.Text.Json.Serialization;

namespace CensusRx.JsonConverters;

public class CensusCollectionConverter<T> : JsonConverter<ICollection<T>>
{
	public override bool CanConvert(Type typeToConvert) =>
		typeToConvert.IsAssignableTo(typeof(ICollection<T>))
		&& typeToConvert != typeof(ICollection<T>);

	public override ICollection<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using var jsonDocument = JsonDocument.ParseValue(ref reader);

		var items = jsonDocument.RootElement.EnumerateObject()
			.First(prop => prop.Value.ValueKind == JsonValueKind.Array)
			.Value.EnumerateArray()
			.Select(element => element.Deserialize<T>(options))
			.Where(item => item is not null)
			.Select(item => item!);

		return (ICollection<T>?)Activator.CreateInstance(typeToConvert, items);
	}

	public override void Write(Utf8JsonWriter writer, ICollection<T> value, JsonSerializerOptions options)
	{
		throw new NotSupportedException();
	}
}