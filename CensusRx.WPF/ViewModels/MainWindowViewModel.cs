using System;
using System.Windows;
using CensusRx.Services;
using ControlzEx.Theming;
using Microsoft.Extensions.Configuration;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class MainWindowViewModel : WindowViewModel
{
	public CharacterSearchViewModel CharacterSearch { get; }
	
	public WeaponSearchViewModel WeaponSearch { get; }

	public ThemeConfigViewModel ThemeConfig { get; }

	public Uri? LastRequest => _lastRequest.Value;

	private readonly ObservableAsPropertyHelper<Uri?> _lastRequest;

	public MainWindowViewModel(
		Application application,
		ICensusClient censusClient,
		ICensusCache censusCache,
		ThemeManager themeManager,
		IConfiguration configuration)
	{
		CharacterSearch = new CharacterSearchViewModel(this, censusClient, censusCache);
		WeaponSearch = new WeaponSearchViewModel(this, censusClient);
		ThemeConfig = new ThemeConfigViewModel(this, application, themeManager, configuration);

		_lastRequest = censusClient.LastRequest
			.ToProperty(this, viewModel => viewModel.LastRequest);
	}
}