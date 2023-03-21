using System.Text.Json.Serialization;
using CensusRx.Interfaces;

namespace CensusRx.Model;

public record ItemType(
		[property: JsonPropertyName("item_type_id")]
		long Id,
		string Name,
		string Code)
	: ICensusObject
{
	public const long ID_WEAPON = 26;
}