using System.Reactive.Disposables;
using DbgCensus.Core.Objects;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;

namespace CensusRx.EventStream;

internal sealed class FacilityStatusService : IFacilityStatusService, IDisposable
{
	public IObservableCache<IFacilityStatusInstance, FacilityIdentifier> Facilities => _facilities;
	private readonly SourceCache<IFacilityStatusInstance, FacilityIdentifier> _facilities;

	private readonly IServiceProvider _serviceProvider;

	private readonly CompositeDisposable _disposable = new();

	public FacilityStatusService(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		_facilities = new SourceCache<IFacilityStatusInstance, FacilityIdentifier>(status => status.Identifier)
			.DisposeWith(_disposable);

		// @todo move this into some hosted init service (EventSocketService?)
		foreach (var world in Enum.GetValues<WorldDefinition>())
		{
			foreach (var zone in Enum.GetValues<ZoneDefinition>())
			{
				// @todo these are all fake ids, needs to be seeded with ids from REST API
				for (uint facility = 1; facility <= 100; ++facility)
				{
					RegisterFacility(new FacilityIdentifier(world, zone, facility));
				}
			}
		}
	}

	public IFacilityStatusInstance RegisterFacility(FacilityIdentifier facilityIdentifier)
	{
		var status = _facilities.Items.FirstOrDefault(status => status.Identifier == facilityIdentifier);

		if (status is not null)
		{
			// We already did that...
			return status;
		}

		status = ActivatorUtilities.CreateInstance<FacilityStatusInstance>(_serviceProvider, facilityIdentifier)
			.DisposeWith(_disposable);

		_facilities.AddOrUpdate(status);

		return status;
	}

	public void Dispose() => _disposable.Dispose();
}