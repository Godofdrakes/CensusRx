using DbgCensus.Core.Objects;

namespace CensusRx.EventStream;

public record FacilityIdentifier(WorldDefinition World, ZoneDefinition Zone, long Facility);

public interface IFacilityStatusInstance
{
	public static FacilityIdentifier GetIdentifier(IFacilityStatusInstance facilityStatus)
	{
		return new FacilityIdentifier(facilityStatus.World, facilityStatus.Zone, facilityStatus.Facility);
	}

	WorldDefinition World { get; }
	ZoneDefinition Zone { get; }
	long Facility { get; }
}

internal class NullFacilityStatusInstance : IFacilityStatusInstance
{
	public WorldDefinition World { get; }
	public ZoneDefinition Zone { get; }
	public long Facility { get; }

	public NullFacilityStatusInstance(WorldDefinition world, ZoneDefinition zone, long facility)
	{
		World = world;
		Zone = zone;
		Facility = facility;
	}
}