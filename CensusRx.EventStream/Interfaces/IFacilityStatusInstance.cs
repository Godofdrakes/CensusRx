using DbgCensus.Core.Objects;

namespace CensusRx.EventStream;

public record FacilityIdentifier(WorldDefinition World, ZoneDefinition Zone, uint Facility)
{
	public static FacilityIdentifier Null(WorldDefinition world, ZoneDefinition zone) => new(world, zone, 0);
}

public interface IFacilityStatusInstance
{
	FacilityIdentifier Identifier { get; }

	FactionDefinition OwningFaction { get; }

	ulong OwningOutfit { get; }
}

internal sealed class NullFacilityStatusInstance : IFacilityStatusInstance
{
	public FacilityIdentifier Identifier { get; }
	public FactionDefinition OwningFaction { get; set; }
	public ulong OwningOutfit { get; set; }

	public NullFacilityStatusInstance(FacilityIdentifier identifier)
	{
		Identifier = identifier;
	}
}