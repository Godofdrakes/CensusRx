using DbgCensus.Core.Objects;

namespace CensusRx.EventStream;

public record ZoneIdentifier(WorldDefinition World, ZoneDefinition Zone);

public interface IZoneStatusInstance
{
	ZoneIdentifier Identifier { get; }
	
	bool IsUnlocked { get; }
}

internal sealed class NullZoneStatusInstance : IZoneStatusInstance
{
	public ZoneIdentifier Identifier { get; }
	public bool IsUnlocked { get; set; } = true;

	public NullZoneStatusInstance(ZoneIdentifier identifier)
	{
		Identifier = identifier;
	}
}