using CensusRx.Interfaces;

namespace CensusRx.Model;

public class Item : ICensusObject
{
	public long ItemId { get; set; }
	public int ItemTypeId { get; set; }
	public int ItemCategoryId { get; set; }
	public bool IsVehicleWeapon { get; set; }
	public LocalizedString Name { get; set; } = LocalizedString.Invalid;
	public LocalizedString Description { get; set; } = LocalizedString.Invalid;
	public int FactionId { get; set; }
	public int ImageSetId { get; set; }
	public int MaxStackSize { get; set; }
	public int ImageId { get; set; }
	public string ImagePath { get; set; } = string.Empty;
	public int SkillSetId { get; set; }
	public bool IsDefaultAttachment { get; set; }

	public long Id => ItemId;
	public Uri ImageUri => new(ImagePath, UriKind.Relative);
}
