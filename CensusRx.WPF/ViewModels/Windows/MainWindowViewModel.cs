using System;
using System.Windows;
using CensusRx.Services;
using CensusRx.WPF.Common;
using ControlzEx.Theming;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class MainWindowViewModel : WindowViewModel
{
	public CharacterSearchViewModel CharacterSearch { get; }
	
	public WeaponSearchViewModel WeaponSearch { get; }

	public Uri? LastRequest => _lastRequest.Value;

	private readonly ObservableAsPropertyHelper<Uri?> _lastRequest;

	public MainWindowViewModel(Application application, IServiceProvider serviceProvider)
	{
		var censusClient = serviceProvider.GetRequiredService<ICensusClient>();
		var censusCache = serviceProvider.GetRequiredService<ICensusCache>();

		CharacterSearch = new CharacterSearchViewModel(this, censusClient, censusCache);
		WeaponSearch = new WeaponSearchViewModel(this, censusClient);

		_lastRequest = censusClient.LastRequest
			.ToProperty(this, viewModel => viewModel.LastRequest);
	}
}