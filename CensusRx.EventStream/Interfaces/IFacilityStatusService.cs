using DbgCensus.Core.Objects;
using DynamicData;

namespace CensusRx.EventStream;

public record FacilityIdentifier(WorldDefinition World, ZoneDefinition Zone, long Facility);

public interface IFacilityStatusService
{
	public IObservableCache<IFacilityStatusInstance, FacilityIdentifier> FacilityStatus { get; }
}