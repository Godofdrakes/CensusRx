using System.Reactive.Disposables;
using System.Reactive.Linq;
using DbgCensus.Core.Objects;
using DbgCensus.EventStream.Abstractions.Objects.Events.Worlds;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace CensusRx.EventStream;

internal sealed class FacilityStatus : ReactiveObject, IFacilityStatus, IDisposable
{
	public FacilityIdentifier Identifier { get; }
	public FactionDefinition OwningFaction => _owningFaction.Value;
	public ulong OwningOutfit => _owningOutfit.Value;

	private readonly ObservableAsPropertyHelper<FactionDefinition> _owningFaction;
	private readonly ObservableAsPropertyHelper<ulong> _owningOutfit;

	private readonly CompositeDisposable _disposable = new();

	public FacilityStatus(
		FacilityIdentifier identifier,
		IPayloadObservable<IFacilityControl> facilityControl,
		ILogger<FacilityStatus> logger)
	{
		Identifier = identifier;

		bool MatchesFacilityIdentifier(IFacilityControl payload) => payload.FacilityID == identifier.Facility;

		_owningFaction = facilityControl.PayloadReceived
			.Where(MatchesFacilityIdentifier)
			.Select(payload => payload.NewFactionID)
			.ToProperty(this, status => status.OwningFaction, initialValue: FactionDefinition.None)
			.DisposeWith(_disposable);

		_owningOutfit = facilityControl.PayloadReceived
			.Where(MatchesFacilityIdentifier)
			.Select(payload => payload.OutfitID)
			.ToProperty(this, status => status.OwningOutfit, initialValue: 0ul)
			.DisposeWith(_disposable);

		this.WhenAnyValue(status => status.OwningFaction)
			.Subscribe(faction => logger.LogDebug("'{Facility}' captured by {Faction}",
				Identifier.Facility, faction))
			.DisposeWith(_disposable);
	}

	public void Dispose() => _disposable.Dispose();
}