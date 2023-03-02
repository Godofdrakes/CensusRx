namespace CensusRx.Model;

public record LocalizedString(string De, string En, string Es, string Fr, string It, string Tr)
{
	public static LocalizedString Invalid => new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
}
