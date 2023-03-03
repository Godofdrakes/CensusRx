using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;
using CensusRx.Interfaces;
using CensusRx.Model;
using CensusRx.ViewModels;
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

			Locator.CurrentMutable.RegisterLazySingleton<ICensusCache<FactionViewModel>>(() =>
			{
				var cache = new CensusCacheViewModel<FactionViewModel>(10);
				var client = Locator.Current.GetServiceChecked<ICensusClient>();
				var service = Locator.Current.GetServiceChecked<ICensusService>();
				client.Get<Faction>(request => request.LimitTo(10))
					.SelectMany(json => json.UnwrapCensusCollection<Faction>())
					.Select(faction => new FactionViewModel(faction, service))
					.Subscribe(cache.Precache);
				return cache;
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
