using System;
using CensusRx.Services;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class MainWindowViewModel : WindowViewModel
{
	public CharacterSearchViewModel CharacterSearch { get; }
	
	public WeaponSearchViewModel WeaponSearch { get; }

	public Uri? LastRequest => _lastRequest.Value;

	private readonly ObservableAsPropertyHelper<Uri?> _lastRequest;

	public MainWindowViewModel(ICensusClient censusClient, ICensusCache censusCache)
	{
		CharacterSearch = new CharacterSearchViewModel(this, censusClient, censusCache);
		WeaponSearch = new WeaponSearchViewModel(this, censusClient);

		_lastRequest = censusClient.LastRequest
			.ToProperty(this, viewModel => viewModel.LastRequest);
	}
}