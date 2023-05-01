using DbgCensus.Core.Objects;
using DynamicData;

namespace CensusRx.EventStream;

public interface IWorldStatusService
{
	public static IWorldStatusService Null { get; } = new NullWorldStatusService();

	IObservableCache<IWorldStatus, WorldDefinition> Worlds { get; }
	IWorldStatus RegisterWorld(WorldDefinition worldDefinition);
}

internal sealed class NullWorldStatusService : IWorldStatusService
{
	public IObservableCache<IWorldStatus, WorldDefinition> Worlds { get; }

	public NullWorldStatusService()
	{
		var worlds = new SourceCache<IWorldStatus, WorldDefinition>(world => world.Identifier);

		foreach (var world in Enum.GetValues<WorldDefinition>())
		{
			worlds.AddOrUpdate(new NullWorldStatus(world)
			{
				IsOnline = (long) world % 2 == 0,
			});
		}

		Worlds = worlds;
	}

	public IWorldStatus RegisterWorld(WorldDefinition worldDefinition)
	{
		throw new NotImplementedException();
	}
}