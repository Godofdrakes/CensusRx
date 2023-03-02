using System.Windows;
using MahApps.Metro.Controls;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class CensusMenuItem : HamburgerMenuItem
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		nameof(ViewModel),
		typeof(IRoutableViewModel),
		typeof(CensusMenuItem),
		new PropertyMetadata(default(IRoutableViewModel)));

	public IRoutableViewModel ViewModel
	{
		get => (IRoutableViewModel) GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}
}
