using System.Reactive.Linq;
using CensusRx.Interfaces;
using CensusRx.Model;
using CensusRx.Services;
using CensusRx.WPF.Interfaces;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class CharacterViewModel : ReactiveObject, ICensusViewModel
{
	public ICensusObject CensusObject => _character;

	public Character Character
	{
		get => _character;
		set => this.RaiseAndSetIfChanged(ref _character, value);
	}

	public Faction? Faction => _faction.Value;

	private Character _character;
	private readonly ObservableAsPropertyHelper<Faction?> _faction;

	public CharacterViewModel(ICensusCache censusCache, Character? character = default)
	{
		this._character = character ?? new Character();
		this._faction = this.WhenAnyValue(model => model.Character.FactionId)
			.Select(id => censusCache.Get<Faction>((long)id))
			.Switch()
			.ToProperty(this, model => model.Faction);
	}
}
