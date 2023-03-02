using CensusRx.Interfaces;

namespace CensusRx.Model;

public class ItemCategory : ICensusObject
{
	public long ItemCategoryId { get; set; }
	public LocalizedString Name { get; set; } = LocalizedString.Invalid;

	public long Id => ItemCategoryId;
}
