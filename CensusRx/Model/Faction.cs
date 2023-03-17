using System.Text.Json.Serialization;
using CensusRx.Converters;
using CensusRx.Interfaces;

namespace CensusRx.Model;

[JsonConverter(typeof(EnumByteConverter))]
public enum FactionId : byte
{
	None,
	VanuSovereignty,
	NewConglomerate,
	TerranRepublic,
	NSOperatives,
}

public class Faction : ICensusObject
{
	public FactionId FactionId { get; set; }

	public LocalizedString Name { get; set; } = LocalizedString.Invalid;
	public long ImageSetId { get; set; }
	public long ImageId { get; set; }
	public string ImagePath { get; set; } = string.Empty;
	public string CodeTag { get; set; } = string.Empty;
	//public int UserSelectable { get; set; }

	public long Id => (long)FactionId;

	public override string ToString() => Name.En;
}
