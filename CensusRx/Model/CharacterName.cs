namespace CensusRx.Model;

public record CharacterName(string First, string FirstLower)
{
	public static CharacterName Invalid => new(string.Empty, string.Empty);
}