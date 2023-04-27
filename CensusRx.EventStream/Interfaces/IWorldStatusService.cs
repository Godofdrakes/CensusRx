using System.Reactive.Linq;
using System.Reactive.Subjects;
using DbgCensus.Core.Objects;
using DynamicData;

namespace CensusRx.EventStream;

public record WorldStatus(WorldDefinition World, bool IsOnline);

public interface IWorldStatusService
{
	public static IWorldStatusService Null { get; } = new NullWorldStatusService();

	IObserver<WorldStatus> WorldStatusChanged { get; }
	IObservableCache<WorldStatus, WorldDefinition> WorldStatusCache { get; }
	IObservable<bool> GetWorldStatus(WorldDefinition world);
}

public class NullWorldStatusService : IWorldStatusService
{
	public IObserver<WorldStatus> WorldStatusChanged { get; }
	public IObservableCache<WorldStatus, WorldDefinition> WorldStatusCache { get; }

	public NullWorldStatusService()
	{
		WorldStatusChanged = new Subject<WorldStatus>();
		WorldStatusCache = new SourceCache<WorldStatus, WorldDefinition>(status => status.World);
	}

	public IObservable<bool> GetWorldStatus(WorldDefinition world) => Observable
		.Return(false).Concat(Observable.Never<bool>());
}