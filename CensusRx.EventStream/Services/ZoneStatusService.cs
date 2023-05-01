using System.Reactive.Disposables;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;

namespace CensusRx.EventStream;

internal sealed class ZoneStatusService : IZoneStatusService, IDisposable
{
	public IObservableCache<IZoneStatus, ZoneIdentifier> Zones => _zones;
	private readonly SourceCache<IZoneStatus, ZoneIdentifier> _zones;

	private readonly IServiceProvider _serviceProvider;
	private readonly CompositeDisposable _disposable = new();

	public ZoneStatusService(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		_zones = new SourceCache<IZoneStatus, ZoneIdentifier>(status => status.Identifier);
	}

	public IZoneStatus RegisterZone(ZoneIdentifier zoneIdentifier)
	{
		var status = _zones.Items.FirstOrDefault(status => status.Identifier == zoneIdentifier);
		if (status is not null)
		{
			// We already did this
			return status;
		}

		status = ActivatorUtilities.CreateInstance<ZoneStatus>(_serviceProvider, zoneIdentifier)
			.DisposeWith(_disposable);
		
		_zones.AddOrUpdate(status);

		return status;
	}

	public void Dispose() => _disposable.Dispose();
}