using CensusRx.Interfaces;
using CensusRx.Model;
using ReactiveUI;
using Splat;

namespace CensusRx.ViewModels;

public class FactionViewModel : ReactiveObject, ICensusViewModel
{
	public Faction Faction { get; }

	public Uri ImageUri => new(CensusService.Endpoint + Faction.ImagePath);

	public ICensusObject CensusObject => Faction;

	private ICensusService CensusService { get; }

	public FactionViewModel(Faction faction, ICensusService? censusService = default)
	{
		Faction = faction;
		CensusService = censusService ?? Locator.Current.GetServiceChecked<ICensusService>();
	}
}