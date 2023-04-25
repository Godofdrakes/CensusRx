using System.Windows;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace CensusRx.WPF.Common;

public static class HostBuilderCensusRxExtensions
{
	public static IHostBuilder ConfigureWpf<TApplication>(this IHostBuilder hostBuilder)
		where TApplication : Application => hostBuilder
		.ConfigureServices(collection => collection.AddTransient<IWpfService, WpfAppInitService>())
		.ConfigureWpf(builder => builder.UseApplication<TApplication>())
		.UseWpfLifetime();

	public static IHostBuilder ConfigureSplat(this IHostBuilder hostBuilder) =>
		hostBuilder.ConfigureServices(collection =>
		{
			collection.UseMicrosoftDependencyResolver();

			var locator = Locator.CurrentMutable;
			locator.InitializeSplat();
			locator.InitializeReactiveUI();
		});
}