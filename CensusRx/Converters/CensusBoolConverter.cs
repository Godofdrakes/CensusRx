using System.Text.Json;
using System.Text.Json.Serialization;

namespace CensusRx.Converters;

public class CensusBoolConverter : JsonConverter<bool>
{
	public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (byte.TryParse(reader.GetString(), out var value))
		{
			return value == 1;
		}

		throw new JsonException();
	}

	public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(Convert.ToByte(value).ToString());
	}
}