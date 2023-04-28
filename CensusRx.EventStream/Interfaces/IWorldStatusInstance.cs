using System.ComponentModel;
using DbgCensus.Core.Objects;

namespace CensusRx.EventStream;

public interface IWorldStatusInstance : INotifyPropertyChanged
{
	WorldDefinition Id { get; }

	bool IsOnline { get; }

	IEnumerable<IZoneStatusInstance> Zones { get; }
}