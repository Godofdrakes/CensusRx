using System.Windows;
using CensusRx.Services;
using CensusRx.WPF.ViewModels;
using ControlzEx.Theming;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace CensusRx.WPF.Views;

[ServiceLifetime(ServiceLifetime.Transient)]
public abstract class WindowView<TViewModel> : MetroWindow, IViewFor<TViewModel>
	where TViewModel : WindowViewModel
{
	/// <summary>
	/// The view model dependency property.
	/// </summary>
	public static readonly DependencyProperty ViewModelProperty =
		DependencyProperty.Register(
			nameof(ViewModel),
			typeof(TViewModel),
			typeof(WindowView<TViewModel>),
			new PropertyMetadata(null));

	public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(
		nameof(Theme),
		typeof(Theme),
		typeof(WindowView<TViewModel>),
		new PropertyMetadata(null));

	public Theme? Theme
	{
		get => (Theme?) GetValue(ThemeProperty);
		set => SetValue(ThemeProperty, value);
	}

	/// <summary>
	/// Gets the binding root view model.
	/// </summary>
	public TViewModel? BindingRoot => ViewModel;

	/// <inheritdoc/>
	public TViewModel? ViewModel
	{
		get => (TViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	/// <inheritdoc/>
	object? IViewFor.ViewModel
	{
		get => ViewModel;
		set => ViewModel = (TViewModel?)value;
	}

	protected WindowView(ThemeManager themeManager)
	{
		this.WhenAnyValue(control => control.ViewModel)
			.BindTo(this, control => control.DataContext);
		this.OneWayBind(ViewModel, model => model.Title, window => window.Title);
		this.OneWayBind(ViewModel, model => model.Theme, window => window.Theme,
			theme => themeManager.GetTheme(theme));
	}
}
