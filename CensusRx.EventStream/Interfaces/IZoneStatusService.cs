using DbgCensus.Core.Objects;
using DynamicData;

namespace CensusRx.EventStream;

public interface IZoneStatusService
{
	public static IZoneStatusService Null { get; } = new NullZoneStatusService();

	IObservableCache<IZoneStatusInstance, ZoneIdentifier> Zones { get; }
}

internal class NullZoneStatusService : IZoneStatusService
{
	public IObservableCache<IZoneStatusInstance, ZoneIdentifier> Zones { get; }

	public NullZoneStatusService()
	{
		var zones = new SourceCache<IZoneStatusInstance, ZoneIdentifier>(IZoneStatusInstance.GetIdentifier);

		zones.AddOrUpdate(Enum.GetValues<WorldDefinition>()
			.SelectMany(world => Enum.GetValues<ZoneDefinition>()
				.Select(zone => new NullZoneStatusInstance(world, zone))));

		Zones = zones;
	}
}