using DbgCensus.Core.Objects;

namespace CensusRx.EventStream;

public record ZoneIdentifier(WorldDefinition World, ZoneDefinition Zone);

public interface IZoneStatus
{
	ZoneIdentifier Identifier { get; }
	
	bool IsUnlocked { get; }
}

internal sealed class NullZoneStatus : IZoneStatus
{
	public ZoneIdentifier Identifier { get; }
	public bool IsUnlocked { get; set; } = true;

	public NullZoneStatus(ZoneIdentifier identifier)
	{
		Identifier = identifier;
	}
}