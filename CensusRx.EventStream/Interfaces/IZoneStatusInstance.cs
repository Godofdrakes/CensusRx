using DbgCensus.Core.Objects;

namespace CensusRx.EventStream;

public interface IZoneStatusInstance
{
	ZoneDefinition Id { get; }
}

internal class NullZoneStatusInstance : IZoneStatusInstance
{
	public ZoneDefinition Id { get; set; }
}