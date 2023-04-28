using DbgCensus.Core.Objects;

namespace CensusRx.EventStream;

public interface IZoneStatusInstance
{
	ZoneDefinition Id { get; }

	IEnumerable<IFacilityStatusInstance> Facilities { get; }
}