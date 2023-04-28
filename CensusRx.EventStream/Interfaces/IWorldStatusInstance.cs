using System.ComponentModel;
using DbgCensus.Core.Objects;
using ReactiveUI;

namespace CensusRx.EventStream;

public interface IWorldStatusInstance : INotifyPropertyChanged
{
	WorldDefinition Id { get; }

	bool IsOnline { get; }
}

internal class NullWorldStatusInstance : ReactiveObject, IWorldStatusInstance
{
	public WorldDefinition Id { get; set; }

	public bool IsOnline { get; set; }
}