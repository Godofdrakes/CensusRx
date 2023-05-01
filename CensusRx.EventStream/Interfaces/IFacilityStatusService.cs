using DbgCensus.Core.Objects;
using DynamicData;

namespace CensusRx.EventStream;

public interface IFacilityStatusService
{
	public static IFacilityStatusService Null { get; } = new NullFacilityStatusService();

	public IObservableCache<IFacilityStatus, FacilityIdentifier> Facilities { get; }
	IFacilityStatus RegisterFacility(FacilityIdentifier facilityIdentifier);
}

internal class NullFacilityStatusService : IFacilityStatusService
{
	public IObservableCache<IFacilityStatus, FacilityIdentifier> Facilities { get; }

	public NullFacilityStatusService()
	{
		var facilities = new SourceCache<IFacilityStatus, FacilityIdentifier>(status => status.Identifier);

		facilities.AddOrUpdate(Enum.GetValues<WorldDefinition>()
			.SelectMany(world => Enum.GetValues<ZoneDefinition>()
				.SelectMany(zone => Enumerable.Range(1, 100)
					.Select(Convert.ToUInt32)
					.Select(facility => new FacilityIdentifier(world, zone, facility))
					.Select(identifier => new NullFacilityStatus(identifier)))));

		Facilities = facilities;
	}

	public IFacilityStatus RegisterFacility(FacilityIdentifier facilityIdentifier)
	{
		throw new NotImplementedException();
	}
}