using System;
using System.Linq;
using System.Reflection;
using CensusRx.Interfaces;
using CensusRx.WPF.Attributes;
using CensusRx.WPF.Interfaces;
using CensusRx.WPF.Services;
using CensusRx.WPF.Views;
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
	public static void Main(string[] args)
	{
		var host = Host.CreateDefaultBuilder(args)
			.ConfigureServices(services =>
			{
				services.AddSingleton<ICensusService, CensusService>();
				services.AddSingleton<ICensusClient, CensusClient>();
				services.AddSingleton<PropertyReferenceRegistry>();
				services.AddViews(Assembly.GetExecutingAssembly());
				services.AddViewModels(Assembly.GetExecutingAssembly());

				// Make splat init using this service collection
				services.UseMicrosoftDependencyResolver();

				var resolver = Locator.CurrentMutable;
				resolver.InitializeSplat();
				resolver.InitializeReactiveUI();
			})
			.ConfigureWpf(builder =>
			{
				builder.UseApplication<App>();
				builder.UseWindow<MainWindowView>();
			})
			.UseWpfLifetime()
			.Build();

		// Make splat resolve using this service provider
		//host.Services.UseMicrosoftDependencyResolver();
		host.Run();
	}

	private static IServiceCollection AddViewModels(this IServiceCollection services, Assembly assembly)
	{
		if (services is null) throw new ArgumentNullException(nameof(services));
		if (assembly is null) throw new ArgumentNullException(nameof(assembly));

		// for each type that implements IViewModel
		bool IsViewModel(TypeInfo ti) => ti.ImplementedInterfaces.Contains(typeof(IViewModel)) && !ti.IsAbstract;
		foreach (var implementationType in assembly.DefinedTypes.Where(IsViewModel))
		{
			services.RegisterType(implementationType, implementationType);
		}

		return services;
	}

	private static IServiceCollection AddViews(this IServiceCollection services, Assembly assembly)
	{
		if (services is null) throw new ArgumentNullException(nameof(services));
		if (assembly is null) throw new ArgumentNullException(nameof(assembly));

		// for each type that implements IViewFor
		bool IsViewFor(TypeInfo ti) => ti.ImplementedInterfaces.Contains(typeof(IViewFor)) && !ti.IsAbstract;
		foreach (var implementationType in assembly.DefinedTypes.Where(IsViewFor))
		{
			// grab the first _implemented_ interface that also implements IViewFor, this should be the expected IViewFor<>
			bool ImplementsViewFor(Type t) => t.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IViewFor));
			var serviceType = implementationType.ImplementedInterfaces.FirstOrDefault(ImplementsViewFor);

			// need to check for null because some classes may implement IViewFor but not IViewFor<T> - we don't care about those
			if (serviceType is null) continue;

			services.RegisterType(serviceType, implementationType);
		}

		return services;
	}

	private static ServiceLifetime GetServiceLifetime(this Type type) =>
		type.GetCustomAttribute<ServiceLifetimeAttribute>()?.Lifetime ?? ServiceLifetime.Transient;

	private static void RegisterType(this IServiceCollection services, Type serviceType, Type implementationType)
	{
		switch (implementationType.GetServiceLifetime())
		{
			case ServiceLifetime.Singleton:
				services.AddSingleton(serviceType, implementationType);
				return;
			case ServiceLifetime.Scoped:
				services.AddScoped(serviceType, implementationType);
				return;
			case ServiceLifetime.Transient:
				services.AddTransient(serviceType, implementationType);
				return;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}