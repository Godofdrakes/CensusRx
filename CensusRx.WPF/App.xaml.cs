using System.Reflection;
using System.Windows;
using CensusRx.Interfaces;
using CensusRx.Model;
using CensusRx.WPF.ViewModels;
using CensusRx.WPF.Views;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Splat;

namespace CensusRx.WPF
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private void App_OnStartup(object sender, StartupEventArgs e)
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile("service.json", true, false)
				.Build();

			var censusClient = new CensusClient(CensusNamespace.PLANETSIDE_PC, config["serviceId"]);

			Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
			Locator.CurrentMutable.RegisterConstant<ICensusClient>(censusClient);

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
}
