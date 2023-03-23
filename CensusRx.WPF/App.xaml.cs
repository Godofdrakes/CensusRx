using System;
using System.Windows;
using CensusRx.Services;
using CensusRx.WPF.ViewModels;
using CensusRx.WPF.Views;
using ControlzEx.Theming;
using Microsoft.Extensions.Hosting;

namespace CensusRx.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
	private IHostEnvironment HostEnvironment { get; }
	private ICensusClient CensusClient { get; }
	public ICensusCache CensusCache { get; }
	private ThemeManager ThemeManager { get; }

	public App(IHostEnvironment hostEnvironment, ICensusClient censusClient, ICensusCache censusCache,
		ThemeManager themeManager)
	{
		this.InitializeComponent();
		this.HostEnvironment = hostEnvironment;
		this.CensusClient = censusClient;
		this.CensusCache = censusCache;
		this.ThemeManager = themeManager;
	}

	private void App_OnStartup(object sender, StartupEventArgs e)
	{
		var isDevEnv = !HostEnvironment.EnvironmentName.Contains("prod", StringComparison.CurrentCultureIgnoreCase);

		this.MainWindow = new MainWindowView
		{
			ViewModel = new MainWindowViewModel(CensusClient, CensusCache)
			{
				Title = isDevEnv
					? $"{HostEnvironment.ApplicationName} ({HostEnvironment.EnvironmentName})"
					: HostEnvironment.ApplicationName,
			},
		};

		this.MainWindow.Show();
	}
}