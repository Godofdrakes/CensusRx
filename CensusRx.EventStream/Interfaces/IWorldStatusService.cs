using DbgCensus.Core.Objects;
using DynamicData;

namespace CensusRx.EventStream;

public record WorldStatus(WorldDefinition World, bool IsOnline);

public interface IWorldStatusService
{
	IObservableCache<IWorldStatusInstance, WorldDefinition> Worlds { get; }
}