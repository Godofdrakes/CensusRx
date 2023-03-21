using System.Text.Json.Serialization;
using CensusRx.Interfaces;

namespace CensusRx.Model;

public class Faction : ICensusObject
{
	public const long None = 0;
	public const long VanuSovereignty = 1;
	public const long NewConglomerate = 2;
	public const long TerranRepublic = 3;
	public const long NSOperatives = 4;

	[JsonPropertyName("faction_id")]
	public long Id { get; set; }

	public LocalizedString Name { get; set; } = LocalizedString.Invalid;
	public long ImageSetId { get; set; }
	public long ImageId { get; set; }
	public string ImagePath { get; set; } = string.Empty;
	public string CodeTag { get; set; } = string.Empty;
	//public int UserSelectable { get; set; }

	public override string ToString()
	{
		if (string.IsNullOrEmpty(Name.En))
		{
			return $"Unknown {{ {nameof(Id)} = {Id} }}";
		}

		return $"{Name.En} {{ {nameof(Id)} = {Id} }}";
	}
}
