using DbgCensus.Core.Objects;

namespace CensusRx.EventStream;

public record ZoneIdentifier(WorldDefinition World, ZoneDefinition Zone);

public interface IZoneStatusInstance
{
	public static ZoneIdentifier GetIdentifier(IZoneStatusInstance zoneStatus)
	{
		return new ZoneIdentifier(zoneStatus.World, zoneStatus.Zone);
	}

	WorldDefinition World { get; }

	ZoneDefinition Zone { get; }
}

internal class NullZoneStatusInstance : IZoneStatusInstance
{
	public WorldDefinition World { get; }
	public ZoneDefinition Zone { get; }

	public NullZoneStatusInstance(WorldDefinition world, ZoneDefinition zone)
	{
		World = world;
		Zone = zone;
	}
}