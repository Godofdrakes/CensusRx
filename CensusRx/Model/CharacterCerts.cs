namespace CensusRx.Model;

public record CharacterCerts(
	int EarnedPoints,
	int GiftedPoints,
	int SpentPoints,
	int AvailablePoints,
	float PercentToNext)
{
	public static CharacterCerts Zero => new(0, 0, 0, 0, 0);
}