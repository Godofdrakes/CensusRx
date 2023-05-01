using System.ComponentModel;
using DbgCensus.Core.Objects;
using ReactiveUI;

namespace CensusRx.EventStream;

public interface IWorldStatus
{
	WorldDefinition Identifier { get; }

	bool IsOnline { get; }
}

internal sealed class NullWorldStatus : ReactiveObject, IWorldStatus
{
	public WorldDefinition Identifier { get; }

	public bool IsOnline { get; set; }

	public NullWorldStatus(WorldDefinition identifier)
	{
		Identifier = identifier;
	}

	public void Dispose() { }
}