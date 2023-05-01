using System.Reactive.Disposables;
using System.Reactive.Linq;
using DbgCensus.Core.Objects;
using DbgCensus.EventStream.Abstractions.Objects.Control;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace CensusRx.EventStream;

internal sealed class WorldStatusInstance : ReactiveObject, IWorldStatusInstance, IDisposable
{
	public WorldDefinition Identifier { get; }

	public bool IsOnline => _isOnline.Value;

	private readonly ObservableAsPropertyHelper<bool> _isOnline;
	private readonly CompositeDisposable _disposable = new();

	public WorldStatusInstance(WorldDefinition world,
		IPayloadObservable<IHeartbeat> heartbeat,
		ILogger<WorldStatusInstance> logger)
	{
		Identifier = world;

		_isOnline = heartbeat.PayloadReceived
			.SelectMany(payload => payload.Online
				.Where(pair => pair.Key.Contains(world.ToString())))
			.Select(pair => pair.Value)
			.ToProperty(this, instance => instance.IsOnline, initialValue: false)
			.DisposeWith(_disposable);

		this.WhenAnyValue(status => status.IsOnline)
			.Subscribe(isOnline => logger.LogDebug("{World} is {IsOnline}",
				world, isOnline ? "Online" : "Offline"))
			.DisposeWith(_disposable);
	}

	public void Dispose() => _disposable.Dispose();
}