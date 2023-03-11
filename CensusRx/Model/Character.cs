using CensusRx.Interfaces;

namespace CensusRx.Model;

public class Character : ICensusObject
{
	public long CharacterId { get; set; }
	public CharacterName Name { get; set; } = CharacterName.Invalid;
	public CharacterCerts Certs { get; set; } = CharacterCerts.Zero;
	public long FactionId { get; set; } = Faction.NONE;

	public long Id => CharacterId;
}