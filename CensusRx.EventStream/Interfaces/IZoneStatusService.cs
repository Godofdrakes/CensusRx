using DbgCensus.Core.Objects;
using DynamicData;

namespace CensusRx.EventStream;

public interface IZoneStatusService
{
	public static IZoneStatusService Null { get; } = new NullZoneStatusService();

	IObservableCache<IZoneStatus, ZoneIdentifier> Zones { get; }
	IZoneStatus RegisterZone(ZoneIdentifier zoneIdentifier);
}

internal sealed class NullZoneStatusService : IZoneStatusService
{
	public IObservableCache<IZoneStatus, ZoneIdentifier> Zones { get; }
	
	public NullZoneStatusService()
	{
		var zones = new SourceCache<IZoneStatus, ZoneIdentifier>(status => status.Identifier);

		zones.AddOrUpdate(Enum.GetValues<WorldDefinition>()
			.SelectMany(world => Enum.GetValues<ZoneDefinition>()
				.Select(zone => new ZoneIdentifier(world, zone))
				.Select(identifier => new NullZoneStatus(identifier))));

		Zones = zones;
	}

	public IZoneStatus RegisterZone(ZoneIdentifier zoneIdentifier)
	{
		throw new NotImplementedException();
	}
}