using System.Reactive.Disposables;
using DbgCensus.Core.Objects;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;

namespace CensusRx.EventStream;

internal sealed class WorldStatusService : IWorldStatusService, IDisposable
{
	public IObservableCache<IWorldStatusInstance, WorldDefinition> Worlds => _worlds;
	private readonly SourceCache<IWorldStatusInstance, WorldDefinition> _worlds;

	private readonly IServiceProvider _serviceProvider;
	private readonly CompositeDisposable _disposable = new();

	public WorldStatusService(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		_worlds = new SourceCache<IWorldStatusInstance, WorldDefinition>(instance => instance.Identifier);

		// @todo move this into some hosted init service (EventSocketService?)
		foreach (var worldDefinition in Enum.GetValues<WorldDefinition>())
		{
			RegisterWorld(worldDefinition);
		}
	}

	public IWorldStatusInstance RegisterWorld(WorldDefinition worldDefinition)
	{
		var status = _worlds.Items.FirstOrDefault(status => status.Identifier == worldDefinition);
		if (status is not null)
		{
			// We already did that
			return status;
		}

		status = ActivatorUtilities.CreateInstance<WorldStatusInstance>(_serviceProvider, worldDefinition)
			.DisposeWith(_disposable);

		_worlds.AddOrUpdate(status);

		return status;
	}

	public void Dispose() => _disposable.Dispose();
}