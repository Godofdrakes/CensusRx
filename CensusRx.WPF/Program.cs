using System;
using System.Reflection;
using CensusRx.Interfaces;
using CensusRx.WPF.Interfaces;
using CensusRx.WPF.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace CensusRx.WPF;

public static class Program
{
	[STAThread]
	public static void Main(string[] args)
	{
		var host = Host.CreateDefaultBuilder(args)
			.ConfigureServices(ConfigureServices)
			.Build();

		// Make splat resolve using this service provider
		host.Services.UseMicrosoftDependencyResolver();

		var app = host.Services.GetRequiredService<App>();

		app.InitializeComponent();
		app.Run();
	}

	private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
	{
		services.AddSingleton<App>();
		services.AddSingleton<ICensusService, CensusService>();
		services.AddSingleton<ICensusClient, CensusClient>();
		services.AddSingleton<PropertyReferenceRegistry>();

		// Make splat init using this service collection
		services.UseMicrosoftDependencyResolver();

		var resolver = Locator.CurrentMutable;
		resolver.InitializeSplat();
		resolver.InitializeReactiveUI();
		resolver.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
	}
}