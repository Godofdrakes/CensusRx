using System.ComponentModel;
using DbgCensus.Core.Objects;
using ReactiveUI;

namespace CensusRx.EventStream;

public interface IWorldStatusInstance
{
	WorldDefinition Identifier { get; }

	bool IsOnline { get; }
}

internal sealed class NullWorldStatusInstance : ReactiveObject, IWorldStatusInstance
{
	public WorldDefinition Identifier { get; }

	public bool IsOnline { get; set; }

	public NullWorldStatusInstance(WorldDefinition identifier)
	{
		Identifier = identifier;
	}

	public void Dispose() { }
}