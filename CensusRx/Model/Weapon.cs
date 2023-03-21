using System.Text.Json.Serialization;
using CensusRx.Interfaces;

namespace CensusRx.Model;

public class Weapon : ICensusObject
{
	[JsonPropertyName("weapon_id")]
	public long Id { get; init; }

	public float TurnModifier { get; init; }
	public float MoveModifier { get; init; }

	public int SprintRecoveryMs { get; init; }
	public int EquipMs { get; init; }
	public int UnequipMs { get; init; }
	public int ToIronSightsMs { get; init; }
	public int FromIronSightsMs { get; init; }
	
	public float MeleeDetectWidth { get; init; }
	public float MeleeDetectHeight { get; init; }

	public override string ToString() => Id.ToString();
}