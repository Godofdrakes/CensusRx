using CensusRx.Interfaces;
using CensusRx.Model;
using ReactiveUI;
using Splat;

namespace CensusRx.ViewModels;

public class CharacterViewModel : ReactiveObject, ICensusViewModel
{
	public Character Character { get; }

	public FactionViewModel? Faction => _faction.Value;

	public ICensusObject CensusObject => Character;

	private readonly ObservableAsPropertyHelper<FactionViewModel?> _faction;

	public CharacterViewModel(Character character)
	{
		var factionCache = Locator.Current.GetServiceChecked<ICensusCache<FactionViewModel>>();

		Character = character;

		_faction = factionCache.Get(character.FactionId)
			.ToProperty(this, model => model.Faction, initialValue: null);
	}
}
