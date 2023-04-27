using System.Collections.Concurrent;
using DbgCensus.Core.Objects;
using DbgCensus.EventStream.Abstractions.Objects.Control;
using DbgCensus.EventStream.EventHandlers.Abstractions;

namespace CensusRx.EventStream;

internal class HeartbeatPayloadHandler : IPayloadHandler<IHeartbeat>
{
	private readonly IWorldStatusService _worldStatusService;

	private readonly IDictionary<string, WorldDefinition> _worldDefinitions;

	public HeartbeatPayloadHandler(IWorldStatusService worldStatusService)
	{
		_worldStatusService = worldStatusService;
		_worldDefinitions = new ConcurrentDictionary<string, WorldDefinition>();
	}

	public Task HandleAsync(IHeartbeat payload, CancellationToken ct = default)
	{
		foreach (var worldStatus in payload.Online.Select(CreateWorldStatus))
		{
			_worldStatusService.WorldStatusChanged.OnNext(worldStatus);
		}

		return Task.CompletedTask;
	}

	private WorldStatus CreateWorldStatus(KeyValuePair<string, bool> pair)
	{
		if (!_worldDefinitions.TryGetValue(pair.Key, out var world))
		{
			world = Enum.GetValues<WorldDefinition>()
				.First(definition => pair.Key.Contains(definition.ToString()));

			_worldDefinitions.Add(pair.Key, world);
		}

		return new WorldStatus(world, pair.Value);
	}
}