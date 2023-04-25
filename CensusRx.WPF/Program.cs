using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using CensusRx.Services;
using CensusRx.WPF.Common;
using CensusRx.WPF.Interfaces;
using ControlzEx.Theming;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace CensusRx.WPF;

public static class Program
{
	[STAThread]
	public static Task Main(string[] args)
	{
		var host = Host.CreateDefaultBuilder(args)
#if DEBUG
			.UseEnvironment(Environments.Development)
#endif
			.ConfigureSplat()
			.ConfigureWpf<App>()
			.ConfigureServices(services =>
			{
				var assembly = Assembly.GetExecutingAssembly();
				services.AddAllServices(assembly);
				services.AddAllViews(assembly);

				// Adding the theme manager instance directly crashes the application
				// I don't know why
				services.AddSingleton(_ => ThemeManager.Current);
			})
			.Build();

		return host
			.ConfigureSplat()
			.RunAsync();
	}

	private static IServiceCollection AddAllViewModels(this IServiceCollection services, Assembly assembly)
	{
		if (services is null) throw new ArgumentNullException(nameof(services));
		if (assembly is null) throw new ArgumentNullException(nameof(assembly));

		// for each type that implements IViewModel
		bool IsViewModel(TypeInfo ti) => ti.ImplementedInterfaces.Contains(typeof(IViewModel)) && !ti.IsAbstract;
		foreach (var implementationType in assembly.DefinedTypes.Where(IsViewModel))
		{
			services.AddService(implementationType, implementationType);
		}

		return services;
	}

	private static IServiceCollection AddAllViews(this IServiceCollection services, Assembly assembly)
	{
		if (services is null) throw new ArgumentNullException(nameof(services));
		if (assembly is null) throw new ArgumentNullException(nameof(assembly));

		// for each type that implements IViewFor
		bool IsViewFor(TypeInfo ti) => ti.ImplementedInterfaces.Contains(typeof(IViewFor)) && !ti.IsAbstract;
		foreach (var implementationType in assembly.DefinedTypes.Where(IsViewFor))
		{
			// grab the first _implemented_ interface that also implements IViewFor, this should be the expected IViewFor<>
			bool ImplementsViewFor(Type t) => t.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IViewFor));
			var serviceType = implementationType.ImplementedInterfaces.FirstOrDefault(ImplementsViewFor)?.GetTypeInfo();

			// need to check for null because some classes may implement IViewFor but not IViewFor<T> - we don't care about those
			if (serviceType is null) continue;

			services.AddService(serviceType, implementationType);
		}

		return services;
	}
}