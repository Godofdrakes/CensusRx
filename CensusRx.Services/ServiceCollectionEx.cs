using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CensusRx.Services;

public static class ServiceCollectionEx
{
	public static ServiceLifetime GetServiceLifetime(this Type type)
	{
		var attribute = type.GetCustomAttribute<ServiceLifetimeAttribute>();
		if (attribute == null)
		{
			throw new ArgumentException($"type {type.FullName} has no ServiceLifetimeAttribute", nameof(type));
		}

		return attribute.Lifetime;
	}

	public static IServiceCollection AddAllServices(this IServiceCollection services, Assembly assembly)
	{
		foreach (var serviceImplementation in assembly.GetServices())
		{
			var serviceInterface = serviceImplementation.GetServiceInterface().GetTypeInfo();
			services.AddService(serviceInterface, serviceImplementation);
		}

		return services;
	}

	public static IServiceCollection AddService(this IServiceCollection services, TypeInfo serviceType, TypeInfo implementationType)
	{
		switch (implementationType.GetServiceLifetime())
		{
			case ServiceLifetime.Singleton:
				services.AddSingleton(serviceType, implementationType);
				break;
			case ServiceLifetime.Scoped:
				services.AddScoped(serviceType, implementationType);
				break;
			case ServiceLifetime.Transient:
				services.AddTransient(serviceType, implementationType);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		if (serviceType.ImplementedInterfaces.Contains(typeof(IHostedService)))
		{
			services.AddTransient(typeof(IHostedService), provider => provider.GetRequiredService(serviceType));
		}

		return services;
	}
}