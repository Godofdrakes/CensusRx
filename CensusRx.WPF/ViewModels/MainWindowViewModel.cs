using System;
using System.Collections.ObjectModel;
using CensusRx.WPF.Interfaces;
using DynamicData;

namespace CensusRx.WPF.ViewModels;

public class MainWindowViewModel : WindowViewModel
{
	public ReadOnlyObservableCollection<CensusMenuItem> NavigationMenuItems { get; }

	private SourceCache<CensusMenuItem, Type> NavigationMenuCache { get; } = new(item => item.ViewModel.GetType());

	public MainWindowViewModel(ICensusClient censusClient)
	{
		NavigationMenuCache.Connect()
			.Bind(out var menuItems)
			.Subscribe();
		
		NavigationMenuCache.Edit(cache =>
		{
			cache.AddOrUpdate(new CensusMenuItem
			{
				Label = "Character",
				ToolTip = "Search for characters",
				ViewModel = new CharacterSearchViewModel(this, censusClient),
			});
		});

		this.NavigationMenuItems = menuItems;
	}
}
