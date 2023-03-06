using CensusRx.Interfaces;
using CensusRx.Model;
using ReactiveUI;

namespace CensusRx.ViewModels;

public class CharacterViewModel : ReactiveObject, ICensusViewModel
{
	public ICensusObject CensusObject => _character;

	public CharacterName Name
	{
		get => _name.Value;
		set => this.RaiseAndSetIfChanged(ref _character.Name, value);
	}

	public long FactionId
	{
		get => _factionId.Value;
		set => this.RaiseAndSetIfChanged(ref _character.FactionId, value);
	}

	private Character _character;

	private ObservableAsPropertyHelper<CharacterName> _name;
	private ObservableAsPropertyHelper<long> _factionId;

	public CharacterViewModel() : this(new Character()) { }

	public CharacterViewModel(Character character)
	{
		_character = character;
		_name = this.WhenAnyValue(model => model._character.Name)
			.ToProperty(this, model => model.Name);
		_factionId = this.WhenAnyValue(model => model._character.FactionId)
			.ToProperty(this, model => model.FactionId);
	}
}
