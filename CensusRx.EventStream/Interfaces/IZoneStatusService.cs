using DbgCensus.Core.Objects;
using DynamicData;

namespace CensusRx.EventStream;

public interface IZoneStatusService
{
	public static IZoneStatusService Null { get; } = new NullZoneStatusService();

	IObservableCache<IZoneStatusInstance, ZoneIdentifier> Zones { get; }
	IZoneStatusInstance RegisterZone(ZoneIdentifier zoneIdentifier);
}

internal sealed class NullZoneStatusService : IZoneStatusService
{
	public IObservableCache<IZoneStatusInstance, ZoneIdentifier> Zones { get; }
	
	public NullZoneStatusService()
	{
		var zones = new SourceCache<IZoneStatusInstance, ZoneIdentifier>(status => status.Identifier);

		zones.AddOrUpdate(Enum.GetValues<WorldDefinition>()
			.SelectMany(world => Enum.GetValues<ZoneDefinition>()
				.Select(zone => new ZoneIdentifier(world, zone))
				.Select(identifier => new NullZoneStatusInstance(identifier))));

		Zones = zones;
	}

	public IZoneStatusInstance RegisterZone(ZoneIdentifier zoneIdentifier)
	{
		throw new NotImplementedException();
	}
}