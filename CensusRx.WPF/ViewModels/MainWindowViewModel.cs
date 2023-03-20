using CensusRx.Services;
using CensusRx.Services.Interfaces;

namespace CensusRx.WPF.ViewModels;

public class MainWindowViewModel : WindowViewModel
{
	public CharacterSearchViewModel CharacterSearch { get; }

	public MainWindowViewModel(ICensusClient censusClient, ICensusCache censusCache)
	{
		CharacterSearch = new CharacterSearchViewModel(this, censusClient, censusCache);
	}
}