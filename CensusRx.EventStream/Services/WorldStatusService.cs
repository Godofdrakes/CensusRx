using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DbgCensus.Core.Objects;
using DynamicData;

namespace CensusRx.EventStream;

internal class WorldStatusService : IWorldStatusService
{
	private readonly Subject<WorldStatus> _worldStatusChanged;

	public IObserver<WorldStatus> WorldStatusChanged => _worldStatusChanged.AsObserver();

	public IObservableCache<WorldStatus, WorldDefinition> WorldStatusCache { get; }

	public WorldStatusService()
	{
		_worldStatusChanged = new Subject<WorldStatus>();
		WorldStatusCache = ObservableChangeSet.Create<WorldStatus, WorldDefinition>(
				list =>
					Enum.GetValues<WorldDefinition>()
						.Select(world => new WorldStatus(world, false))
						.ToObservable()
						.Concat(_worldStatusChanged)
						.Subscribe(list.AddOrUpdate),
				status => status.World)
			.AsObservableCache();
	}

	public IObservable<bool> GetWorldStatus(WorldDefinition world) => WorldStatusCache
		.Connect()
		.WatchValue(world)
		.Select(status => status.IsOnline)
		.DistinctUntilChanged();
}