using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

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
			var serviceInterface = serviceImplementation.GetServiceInterface();
			services.AddService(serviceInterface, serviceImplementation);
		}

		return services;
	}

	public static IServiceCollection AddService(this IServiceCollection services, Type serviceType,
		Type implementationType)
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

		return services;
	}

	public static IServiceCollection AddServiceInterfaces(this IServiceCollection services,
		TypeInfo implementationType)
	{
		object? GetImplementationType(IServiceProvider provider) => provider.GetService(implementationType);

		bool IsServiceInterface(MemberInfo type) => type.GetCustomAttribute<ServiceInterfaceAttribute>() is not null;
		foreach (var serviceType in implementationType.ImplementedInterfaces.Where(IsServiceInterface))
		{
			services.AddTransient(serviceType, GetImplementationType!);
		}

		return services;
	}
}