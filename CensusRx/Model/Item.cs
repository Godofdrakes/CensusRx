using System.Text.Json.Serialization;
using CensusRx.Converters;
using CensusRx.Interfaces;

namespace CensusRx.Model;

public class Item : ICensusObject
{
	[JsonPropertyName("item_id")]
	public long Id { get; set; }

	public int ItemTypeId { get; set; }
	public int ItemCategoryId { get; set; }

	public LocalizedString Name { get; set; } = LocalizedString.Invalid;
	public LocalizedString Description { get; set; } = LocalizedString.Invalid;
	public int FactionId { get; set; }
	public int ImageSetId { get; set; }
	public int MaxStackSize { get; set; }
	public int ImageId { get; set; }
	public int SkillSetId { get; set; }
	
	public bool IsVehicleWeapon { get; set; }
	public bool IsDefaultAttachment { get; set; }
	
	public string ImagePath { get; set; } = string.Empty;

	public Weapon? Weapon { get; init; }
	public WeaponDatasheet? WeaponDatasheet { get; init; }

	public override string ToString()
	{
		if (string.IsNullOrEmpty(Name.En))
		{
			return $"Unknown {{ {nameof(Id)} = {Id} }}";
		}

		return $"{Name.En} {{ {nameof(Id)} = {Id} }}";
	}
}
