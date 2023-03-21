using System.Text.Json.Serialization;
using CensusRx.Interfaces;

namespace CensusRx.Model;

public class Character : ICensusObject
{
	[JsonPropertyName("character_id")]
	public long Id { get; init; }

	public CharacterName Name { get; init; } = CharacterName.Invalid;
	public CharacterCerts Certs { get; init; } = CharacterCerts.Zero;
	public long FactionId { get; init; } = Faction.None;

	public override string ToString()
	{
		if (string.IsNullOrEmpty(Name.First))
		{
			return $"Unknown {{ {nameof(Id)} = {Id} }}";
		}

		return $"{Name.First} {{ {nameof(Id)} = {Id} }}";
	}
}