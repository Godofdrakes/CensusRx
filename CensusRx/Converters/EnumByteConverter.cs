using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CensusRx.Converters;

public class EnumByteConverter : JsonConverter<Enum>
{
	public override bool CanConvert(Type typeToConvert)
	{
		return typeToConvert.IsEnum && typeToConvert.GetEnumUnderlyingType() == typeof(byte);
	}

	public override Enum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = byte.Parse(reader.GetString()!);

		if (Enum.IsDefined(typeToConvert, value))
		{
			return (Enum)Enum.ToObject(typeToConvert, value);
		}

		throw new InvalidEnumArgumentException(reader.Position.ToString(), value, typeToConvert);
	}

	public override void Write(Utf8JsonWriter writer, Enum value, JsonSerializerOptions options)
	{
		writer.WriteNumberValue(Convert.ToByte(value));
	}
}