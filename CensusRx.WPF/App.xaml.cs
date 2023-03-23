using System;
using System.Windows;
using CensusRx.Services;
using CensusRx.WPF.ViewModels;
using CensusRx.WPF.Views;
using ControlzEx.Theming;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CensusRx.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
	private IHostEnvironment HostEnvironment { get; }
	private ICensusClient CensusClient { get; }
	private ICensusCache CensusCache { get; }
	private ThemeManager ThemeManager { get; }
	private IConfiguration Configuration { get; }

	public App(
		IHostEnvironment hostEnvironment,
		ICensusClient censusClient,
		ICensusCache censusCache,
		ThemeManager themeManager,
		IConfiguration configuration)
	{
		this.InitializeComponent();
		this.HostEnvironment = hostEnvironment;
		this.CensusClient = censusClient;
		this.CensusCache = censusCache;
		this.ThemeManager = themeManager;
		this.Configuration = configuration;
	}

	private void App_OnStartup(object sender, StartupEventArgs e)
	{
		var isDevEnv = !HostEnvironment.EnvironmentName.Contains("prod", StringComparison.CurrentCultureIgnoreCase);

		this.MainWindow = new MainWindowView
		{
			ViewModel = new MainWindowViewModel(this, CensusClient, CensusCache, ThemeManager, Configuration)
			{
				Title = isDevEnv
					? $"{HostEnvironment.ApplicationName} ({HostEnvironment.EnvironmentName})"
					: HostEnvironment.ApplicationName,
			},
		};

		this.MainWindow.Show();
	}
}