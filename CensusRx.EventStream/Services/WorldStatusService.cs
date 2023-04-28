using System.Collections.Concurrent;
using System.Reactive.Linq;
using DbgCensus.Core.Objects;
using DbgCensus.EventStream.Abstractions.Objects.Control;
using DbgCensus.EventStream.Abstractions.Objects.Events.Worlds;
using DynamicData;
using DynamicData.Binding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace CensusRx.EventStream;

internal class WorldStatusService : IWorldStatusService
{
	public IObservableCache<IWorldStatusInstance, WorldDefinition> Worlds => _worlds;

	private readonly SourceCache<IWorldStatusInstance, WorldDefinition> _worlds;
	private readonly IDictionary<string, WorldDefinition> _worldDefinitions;

	public WorldStatusService(
		ILogger<WorldStatusService> logger,
		IPayloadObservable<IHeartbeat> heartbeat,
		ILoggerFactory loggerFactory)
	{
		_worldDefinitions = new ConcurrentDictionary<string, WorldDefinition>();
		_worlds = new SourceCache<IWorldStatusInstance, WorldDefinition>(instance => instance.Id);

		foreach (var worldDefinition in Enum.GetValues<WorldDefinition>())
		{
			var worldStatus = new WorldStatusStatusInstance(
				loggerFactory.CreateLogger<WorldStatusStatusInstance>(),
				heartbeat,
				worldDefinition);

			_worlds.AddOrUpdate(worldStatus);

			if (logger.IsEnabled(LogLevel.Debug))
			{
				worldStatus.WhenAnyValue(world => world.IsOnline)
					.Subscribe(isOnline => logger.LogInformation(
						"{World} is {IsOnline}",
						worldDefinition,
						isOnline ? "Online" : "Offline"));
			}
		}
	}

	private static IWorldStatusInstance CreateWorldStatusInstance(
		IServiceProvider serviceProvider, WorldDefinition worldDefinition) =>
		ActivatorUtilities.CreateInstance<WorldStatusStatusInstance>(
			serviceProvider, worldDefinition);

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