using System.Reactive.Linq;
using DbgCensus.Core.Objects;
using DynamicData;

namespace CensusRx.EventStream;

public record WorldStatus(WorldDefinition World, bool IsOnline);

public interface IWorldStatusService
{
	public static IWorldStatusService Null { get; } = new NullWorldStatusService();

	IObservableCache<IWorldStatusInstance, WorldDefinition> Worlds { get; }
}

internal class NullWorldStatusService : IWorldStatusService
{
	public IObservableCache<IWorldStatusInstance, WorldDefinition> Worlds { get; }

	public NullWorldStatusService()
	{
		var worlds = new SourceCache<IWorldStatusInstance, WorldDefinition>(world => world.Id);

		foreach (var world in Enum.GetValues<WorldDefinition>())
		{
			worlds.AddOrUpdate(new NullWorldStatusInstance
			{
				Id = world,
				IsOnline = (long) world % 2 == 0,
			});
		}

		Worlds = worlds;
	}
}