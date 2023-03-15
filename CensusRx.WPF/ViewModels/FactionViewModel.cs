using CensusRx.Interfaces;
using CensusRx.Model;
using CensusRx.WPF.Interfaces;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class FactionViewModel : ReactiveObject, ICensusViewModel
{
	public ICensusObject CensusObject => _faction;

	public Faction Faction
	{
		get => _faction;
		set => this.RaiseAndSetIfChanged(ref _faction, value);
	}

	private Faction _faction;

	public FactionViewModel(Faction? faction = default)
	{
		_faction = faction ?? new Faction();
	}
}