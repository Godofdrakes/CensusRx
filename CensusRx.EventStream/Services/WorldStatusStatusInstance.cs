using DbgCensus.Core.Objects;
using DynamicData.Binding;
using ReactiveUI;

namespace CensusRx.EventStream;

internal class WorldStatusStatusInstance : ReactiveObject, IWorldStatusInstance
{
	private readonly ObservableCollectionExtended<IZoneStatusInstance> _zones;
	private readonly ObservableAsPropertyHelper<bool> _isOnline;

	public WorldDefinition Id { get; }

	public bool IsOnline => _isOnline.Value;

	public IEnumerable<IZoneStatusInstance> Zones => _zones;

	public WorldStatusStatusInstance(WorldDefinition world, IObservable<bool> isOnlineChanged)
	{
		_zones = new ObservableCollectionExtended<IZoneStatusInstance>();
		_isOnline = isOnlineChanged
			.ToProperty(this, instance => instance.IsOnline, initialValue: false);

		Id = world;
	}
}