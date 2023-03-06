using CensusRx.Interfaces;

namespace CensusRx.Model;

public class Character : ICensusObject
{
	public long CharacterId;
	public CharacterName Name = CharacterName.Invalid;
	public CharacterCerts Certs = CharacterCerts.Zero;
	public long FactionId = Faction.NONE;

	public long Id => CharacterId;
}