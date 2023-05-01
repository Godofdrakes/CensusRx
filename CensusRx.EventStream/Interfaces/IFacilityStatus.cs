using DbgCensus.Core.Objects;

namespace CensusRx.EventStream;

public record FacilityIdentifier(WorldDefinition World, ZoneDefinition Zone, uint Facility)
{
	public static FacilityIdentifier Null(WorldDefinition world, ZoneDefinition zone) => new(world, zone, 0);
}

public interface IFacilityStatus
{
	FacilityIdentifier Identifier { get; }

	FactionDefinition OwningFaction { get; }

	ulong OwningOutfit { get; }
}

internal sealed class NullFacilityStatus : IFacilityStatus
{
	public FacilityIdentifier Identifier { get; }
	public FactionDefinition OwningFaction { get; set; }
	public ulong OwningOutfit { get; set; }

	public NullFacilityStatus(FacilityIdentifier identifier)
	{
		Identifier = identifier;
	}
}