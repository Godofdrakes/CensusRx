using System.Windows;
using CensusRx.WPF.ViewModels;
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

	protected WindowView()
	{
		this.WhenActivated(d => this.DataContext = this.ViewModel);
	}
}
