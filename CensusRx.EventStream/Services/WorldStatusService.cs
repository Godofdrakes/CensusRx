using System.Collections.Concurrent;
using System.Reactive.Linq;
using DbgCensus.Core.Objects;
using DbgCensus.EventStream.Abstractions.Objects.Control;
using DynamicData;
using DynamicData.Binding;
using Microsoft.Extensions.Logging;

namespace CensusRx.EventStream;

internal class WorldStatusService : IWorldStatusService
{
	public IObservableCache<IWorldStatusInstance, WorldDefinition> Worlds => _worlds;

	private readonly SourceCache<IWorldStatusInstance, WorldDefinition> _worlds;
	private readonly IDictionary<string, WorldDefinition> _worldDefinitions;
	private readonly ILogger<WorldStatusService> _logger;

	public WorldStatusService(IPayloadObservable<IHeartbeat> heartbeatPayload, ILogger<WorldStatusService> logger)
	{
		_logger = logger;
		_worldDefinitions = new ConcurrentDictionary<string, WorldDefinition>();
		_worlds = new SourceCache<IWorldStatusInstance, WorldDefinition>(instance => instance.Id);

		var worldStatusChanged = heartbeatPayload.PayloadReceived
			.SelectMany(heartbeat => heartbeat.Online.Select(CreateWorldStatus));

		_worlds.AddOrUpdate(Enum.GetValues<WorldDefinition>()
			.Select(world => new WorldStatusStatusInstance(world, worldStatusChanged
				.Where(status => status.World == world)
				.Select(status => status.IsOnline))));

		if (logger.IsEnabled(LogLevel.Information))
		{
			void LogWorldStatusChanged(PropertyValue<IWorldStatusInstance, bool> prop) =>
				_logger.LogInformation("{World} is {Status}",
					prop.Sender.Id, prop.Value ? "Online" : "Offline");

			_worlds.Connect()
				.WhenPropertyChanged(world => world.IsOnline)
				.Subscribe(LogWorldStatusChanged);
		}
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