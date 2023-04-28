using System.Reactive.Linq;
using DbgCensus.Core.Objects;
using DbgCensus.EventStream.Abstractions.Objects.Control;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace CensusRx.EventStream;

internal class WorldStatusStatusInstance : ReactiveObject, IWorldStatusInstance
{
	private readonly ObservableAsPropertyHelper<bool> _isOnline;

	public WorldDefinition Id { get; }

	public bool IsOnline => _isOnline.Value;

	public WorldStatusStatusInstance(
		ILogger<WorldStatusStatusInstance> logger,
		IPayloadObservable<IHeartbeat> heartbeat,
		WorldDefinition world)
	{
		_isOnline = heartbeat.PayloadReceived
			.SelectMany(payload => payload.Online
				.Where(pair => pair.Key.Contains(world.ToString())))
			.Select(pair => pair.Value)
			.ToProperty(this, instance => instance.IsOnline, initialValue: false);

		Id = world;
	}
}