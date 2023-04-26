using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace CensusRx.EventStream.WPF
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private readonly IServiceProvider _serviceProvider;

		public App(IServiceProvider serviceProvider)
		{
			this._serviceProvider = serviceProvider;
		}

		private void App_OnStartup(object sender, StartupEventArgs e)
		{
			this.MainWindow = new MainWindow()
			{
				ViewModel = ActivatorUtilities
					.CreateInstance<MainWindowViewModel>(_serviceProvider),
			};

			this.MainWindow.Show();
		}
	}
}