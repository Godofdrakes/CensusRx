using System.Reactive.Linq;
using CensusRx.Interfaces;
using CensusRx.Model;
using CensusRx.Services;
using CensusRx.Services.Interfaces;
using CensusRx.WPF.Interfaces;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class CharacterViewModel : ReactiveObject, ICensusViewModel<Character>
{
	public Character CensusObject
	{
		get => _censusObject;
		set => this.RaiseAndSetIfChanged(ref _censusObject, value);
	}

	public Faction? Faction => _faction.Value;

	private Character _censusObject;
	private readonly ObservableAsPropertyHelper<Faction?> _faction;

	public CharacterViewModel(ICensusCache censusCache, Character? character = default)
	{
		this._censusObject = character ?? new Character();
		this._faction = this.WhenAnyValue(model => model.CensusObject.FactionId)
			.Select(id => censusCache.Get<Faction>((long)id))
			.Switch()
			.ToProperty(this, model => model.Faction);
	}
}
