using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MahApps.Metro.Controls;
using ReactiveUI;

namespace CensusRx.WPF.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindowView
{
	public MainWindowView()
	{
		InitializeComponent();

		this.WhenActivated(d =>
		{
			// XAML bindings don't work on hamburger menu items, must be done in code
			this.BindCommand(ViewModel, 
				model => model.ResetViewModel,
				view => view.CharacterItem,
				model => model.CharacterSearch);

			var firstItem = this.WhenAnyValue(view => view.HamburgerMenu.ItemsSource)
				.OfType<ICollection<HamburgerMenuItemBase>>()
				.Select(items => items.FirstOrDefault())
				.WhereNotNull();

			firstItem.BindTo(this, view => view.HamburgerMenu.SelectedItem);

			// setting the selected item doesn't automatically invoke commands
			firstItem.OfType<HamburgerMenuItem>()
				.Subscribe(item => item.RaiseCommand());
		});
	}
}