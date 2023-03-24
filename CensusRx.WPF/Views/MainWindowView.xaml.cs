using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ControlzEx.Theming;
using MahApps.Metro.Controls;
using ReactiveUI;

namespace CensusRx.WPF.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindowView
{
	public MainWindowView(ThemeManager themeManager) : base(themeManager)
	{
		InitializeComponent();

		this.WhenActivated(d =>
		{
			// XAML bindings don't work on hamburger menu items, must be done in code
			this.BindCommand(ViewModel, 
				model => model.ResetViewModel,
				view => view.CharacterSearch,
				model => model.CharacterSearch);
			this.BindCommand(ViewModel,
				model => model.ResetViewModel,
				view => view.WeaponSearch,
				model => model.WeaponSearch);

			this.OneWayBind(ViewModel, model => model.Router, view => view.RoutedViewHost.Router);
			this.OneWayBind(ViewModel, model => model.LastRequest, view => view.LastRequest.Text);

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