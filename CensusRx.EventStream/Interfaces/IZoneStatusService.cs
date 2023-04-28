using DbgCensus.Core.Objects;
using DynamicData;

namespace CensusRx.EventStream;

public interface IZoneStatusService
{
	public static IZoneStatusService Null { get; }

	IObservableCache<IZoneStatusInstance, ZoneDefinition> Zones { get; }
}

internal class NullZoneStatusService : IZoneStatusService
{
	public IObservableCache<IZoneStatusInstance, ZoneDefinition> Zones { get; }

	public NullZoneStatusService()
	{
		var zones = new SourceCache<IZoneStatusInstance, ZoneDefinition>(zone => zone.Id);

		foreach (var zoneDefinition in Enum.GetValues<ZoneDefinition>())
		{
			zones.AddOrUpdate(new NullZoneStatusInstance()
			{
				Id = zoneDefinition,
			});
		}

		Zones = zones;
	}
}