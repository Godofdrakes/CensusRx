using System.Text.Json.Serialization;
using CensusRx.Interfaces;

namespace CensusRx.Model;

public class Character : ICensusObject
{
	[JsonPropertyName("character_id")]
	public long Id { get; set; }

	public CharacterName Name { get; set; } = CharacterName.Invalid;
	public CharacterCerts Certs { get; set; } = CharacterCerts.Zero;
	public long FactionId { get; set; } = Faction.NONE;

	public override string ToString() => Name.First;
}