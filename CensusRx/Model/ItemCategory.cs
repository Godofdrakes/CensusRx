using System.Text.Json.Serialization;
using CensusRx.Interfaces;

namespace CensusRx.Model;

public class ItemCategory : ICensusObject
{
	[JsonPropertyName("item_category_id")]
	public long Id { get; set; }

	public LocalizedString Name { get; set; } = LocalizedString.Invalid;
}
