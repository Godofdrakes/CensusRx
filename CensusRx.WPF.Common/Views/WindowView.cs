using System.Reactive.Disposables;
using System.Windows;
using MahApps.Metro.Controls;
using ReactiveUI;

namespace CensusRx.WPF.Common;

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
	public TViewModel? BindingRoot
	{
		get => ViewModel;
	}

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
		this.WhenActivated(d =>
		{
			this.WhenAnyValue(control => control.ViewModel)
				.BindTo(this, control => control.DataContext)
				.DisposeWith(d);
		});
	}
}