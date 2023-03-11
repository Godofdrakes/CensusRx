using CensusRx.Interfaces;
using CensusRx.Model;
using ReactiveUI;

namespace CensusRx.ViewModels;

public class CharacterViewModel : ReactiveObject, ICensusViewModel
{
	public ICensusObject CensusObject => _character;

	public Character Character
	{
		get => _character;
		set => this.RaiseAndSetIfChanged(ref _character, value);
	}

	public CharacterName Name => _name.Value;
	public long FactionId => _factionId.Value;

	private Character _character;
	private ObservableAsPropertyHelper<CharacterName> _name;
	private ObservableAsPropertyHelper<long> _factionId;

	public CharacterViewModel() : this(new Character()) { }

	public CharacterViewModel(Character character)
	{
		_character = character;
		_name = this.WhenAnyValue(model => model.Character.Name)
			.ToProperty(this, model => model.Name);
		_factionId = this.WhenAnyValue(model => model.Character.FactionId)
			.ToProperty(this, model => model.FactionId);
	}
}
