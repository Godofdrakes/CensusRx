using DbgCensus.Core.Objects;
using DynamicData;

namespace CensusRx.EventStream;

public interface IFacilityStatusService
{
	public static IFacilityStatusService Null { get; } = new NullFacilityStatusService();

	public IObservableCache<IFacilityStatusInstance, FacilityIdentifier> Facilities { get; }
}

internal class NullFacilityStatusService : IFacilityStatusService
{
	public IObservableCache<IFacilityStatusInstance, FacilityIdentifier> Facilities { get; }

	public NullFacilityStatusService()
	{
		var facilities = new SourceCache<IFacilityStatusInstance, FacilityIdentifier>(
			IFacilityStatusInstance.GetIdentifier);

		facilities.AddOrUpdate(Enum.GetValues<WorldDefinition>()
			.SelectMany(world => Enum.GetValues<ZoneDefinition>()
				.SelectMany(zone => Enumerable.Range(1, 100)
					.Select(facility => new NullFacilityStatusInstance(world, zone, facility)))));

		Facilities = facilities;
	}
}