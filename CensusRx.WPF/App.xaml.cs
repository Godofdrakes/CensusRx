using System;
using System.Windows;
using CensusRx.WPF.Interfaces;
using CensusRx.WPF.ViewModels;
using CensusRx.WPF.Views;
using Microsoft.Extensions.Hosting;

namespace CensusRx.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
	private IHostEnvironment HostEnvironment { get; }
	private ICensusClient CensusClient { get; }

	public App(IHostEnvironment hostEnvironment, ICensusClient censusClient)
	{
		this.InitializeComponent();
		this.HostEnvironment = hostEnvironment;
		this.CensusClient = censusClient;
	}

	private void App_OnStartup(object sender, StartupEventArgs e)
	{
		var isDevEnv = !HostEnvironment.EnvironmentName.Contains("prod", StringComparison.CurrentCultureIgnoreCase);

		this.MainWindow = new MainWindowView
		{
			ViewModel = new MainWindowViewModel(CensusClient)
			{
				Title = isDevEnv
					? $"{HostEnvironment.ApplicationName} ({HostEnvironment.EnvironmentName})"
					: HostEnvironment.ApplicationName,
			},
		};

		this.MainWindow.Show();
	}
}