using CensusRx.Interfaces;
using CensusRx.Model;
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

	private Character _character;

	public CharacterViewModel(Character? character = default)
	{
		_character = character ?? new Character();
	}
}
