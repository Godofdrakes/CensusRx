using System.Text.Json.Serialization;
using CensusRx.Interfaces;

namespace CensusRx.Model;

public class Character : ICensusObject
{
	[JsonPropertyName("character_id")]
	public long Id { get; init; }

	public CharacterName Name { get; init; } = CharacterName.Invalid;
	public CharacterCerts Certs { get; init; } = CharacterCerts.Zero;
	public FactionId FactionId { get; init; } = FactionId.None;

	public override string ToString() => Name.First;
}