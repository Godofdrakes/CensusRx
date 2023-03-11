namespace CensusRx.Model;

public record CharacterName(string First, string FirstLower)
{
	public static CharacterName FromString(string name) => new(name, name.ToLower());
	public static CharacterName Invalid => new(string.Empty, string.Empty);
}