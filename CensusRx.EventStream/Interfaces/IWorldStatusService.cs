using System.Reactive.Linq;
using DbgCensus.Core.Objects;
using DynamicData;

namespace CensusRx.EventStream;

public record WorldStatus(WorldDefinition World, bool IsOnline);

public interface IWorldStatusService
{
	public static IWorldStatusService Null { get; } = new NullWorldStatusService();

	IObservableCache<IWorldStatusInstance, WorldDefinition> Worlds { get; }
	IWorldStatusInstance RegisterWorld(WorldDefinition worldDefinition);
}

internal sealed class NullWorldStatusService : IWorldStatusService
{
	public IObservableCache<IWorldStatusInstance, WorldDefinition> Worlds { get; }

	public NullWorldStatusService()
	{
		var worlds = new SourceCache<IWorldStatusInstance, WorldDefinition>(world => world.Identifier);

		foreach (var world in Enum.GetValues<WorldDefinition>())
		{
			worlds.AddOrUpdate(new NullWorldStatusInstance(world)
			{
				IsOnline = (long) world % 2 == 0,
			});
		}

		Worlds = worlds;
	}

	public IWorldStatusInstance RegisterWorld(WorldDefinition worldDefinition)
	{
		throw new NotImplementedException();
	}
}