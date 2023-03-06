namespace CensusRx.Model;

public record CharacterName(string First, string FirstLower)
{
	public CharacterName(string Name) : this(Name, Name.ToLower()) { }
	public static CharacterName Invalid => new(string.Empty, string.Empty);
}