using System.Text.Json.Serialization;
using CensusRx.Interfaces;

namespace CensusRx.Model;

public class WeaponDatasheet : ICensusObject
{
	[JsonPropertyName("item_id")]
	public long Id { get; init; }

	public int Damage { get; init; }
	public int DamageMin { get; init; }
	public int DamageMax { get; init; }

	public float FireCone { get; init; }
	public float FireConeMin { get; init; }
	public float FireConeMax { get; init; }

	public int ReloadMs { get; init; }
	public int ReloadMsMin { get; init; }
	public int ReloadMsMax { get; init; }

	public int ClipSize { get; init; }
	public int Capacity { get; set; }

	public LocalizedString Range { get; init; } = LocalizedString.Invalid;

	public override string ToString()
	{
		return $"{nameof(WeaponDatasheet)} {{ {nameof(Id)} = {Id} }}";
	}
}