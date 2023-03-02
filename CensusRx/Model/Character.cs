using CensusRx.Interfaces;

namespace CensusRx.Model;

public class Character : ICensusObject
{
	public record CharacterName(string First, string FirstLower)
	{
		public static CharacterName Invalid => new(string.Empty, string.Empty);
	}

	public record CharacterCerts(
		int EarnedPoints,
		int GiftedPoints,
		int SpentPoints,
		int AvailablePoints,
		float PercentToNext)
	{
		public static CharacterCerts Zero => new(0, 0, 0, 0, 0);
	}

	public long CharacterId { get; set; }
	public CharacterName Name { get; set; } = CharacterName.Invalid;
	public CharacterCerts Certs { get; set; } = CharacterCerts.Zero;
	public long FactionId { get; set; } = Faction.NONE;

	public long Id => CharacterId;
}