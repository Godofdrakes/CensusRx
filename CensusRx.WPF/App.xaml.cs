﻿using System;
using System.Windows;
using CensusRx.WPF.ViewModels;
using CensusRx.WPF.Views;
using ControlzEx.Theming;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CensusRx.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
	private IHostEnvironment HostEnvironment { get; }
	private IServiceProvider ServiceProvider { get; }

	public App(
		IHostEnvironment hostEnvironment,
		IServiceProvider serviceProvider)
	{
		this.InitializeComponent();
		this.HostEnvironment = hostEnvironment;
		this.ServiceProvider = serviceProvider;
	}

	private void App_OnStartup(object sender, StartupEventArgs e)
	{
		var themeManager = ServiceProvider.GetRequiredService<ThemeManager>();

		var isDevEnv = !HostEnvironment.EnvironmentName.Contains("prod", StringComparison.CurrentCultureIgnoreCase);

		this.MainWindow = new MainWindowView(themeManager)
		{
			ViewModel = new MainWindowViewModel(this, ServiceProvider)
			{
				Title = isDevEnv
					? $"{HostEnvironment.ApplicationName} ({HostEnvironment.EnvironmentName})"
					: HostEnvironment.ApplicationName,
			},
		};

		this.MainWindow.Show();
	}
}