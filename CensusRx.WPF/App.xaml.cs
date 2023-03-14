using System.Reflection;
using System.Windows;
using CensusRx.WPF.ViewModels;
using CensusRx.WPF.Views;
using ReactiveUI;
using Splat;

namespace CensusRx.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
	private void App_OnStartup(object sender, StartupEventArgs e)
	{
		var mainWindow = new MainWindowView
		{
			ViewModel = new MainWindowViewModel
			{
				Title = "CensusRx.WPF",
			},
		};

		this.MainWindow = mainWindow;
		this.MainWindow.Show();
	}
}