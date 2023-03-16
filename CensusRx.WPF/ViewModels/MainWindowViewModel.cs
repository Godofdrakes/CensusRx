using CensusRx.Services;

namespace CensusRx.WPF.ViewModels;

public class MainWindowViewModel : WindowViewModel
{
	public CharacterSearchViewModel CharacterSearch { get; }

	public MainWindowViewModel(ICensusClient censusClient)
	{
		CharacterSearch = new CharacterSearchViewModel(this, censusClient);
	}
}