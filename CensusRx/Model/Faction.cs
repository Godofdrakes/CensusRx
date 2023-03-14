using CensusRx.Interfaces;

namespace CensusRx.Model;

public class Faction : ICensusObject
{
	public const long NONE = 0;
	public const long VS = 1;
	public const long NC = 2;
	public const long TR = 3;
	public const long NSO = 4;

	public long FactionId { get; set; }
	public LocalizedString Name { get; set; } = LocalizedString.Invalid;
	public long ImageSetId { get; set; }
	public long ImageId { get; set; }
	public string ImagePath { get; set; } = string.Empty;
	public string CodeTag { get; set; } = string.Empty;
	//public int UserSelectable { get; set; }

	public long Id => FactionId;

	public override string ToString() => Name.En;
}
