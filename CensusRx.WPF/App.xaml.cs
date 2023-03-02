using System.IO;
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
			Locator.CurrentMutable.RegisterLazySingleton<ICensusService>(() =>
			{
				var basePath = Directory.GetCurrentDirectory();
				var config = new ConfigurationBuilder()
					.SetBasePath(basePath)
					.AddJsonFile("appsettings.json", false, true)
					.AddUserSecrets(Assembly.GetExecutingAssembly())
					.Build();

				return new CensusServiceViewModel(config);
			});

			Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
			Locator.CurrentMutable.RegisterLazySingleton<ICensusClient>(() => new CensusClient());

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
