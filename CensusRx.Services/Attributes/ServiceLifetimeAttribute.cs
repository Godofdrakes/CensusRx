using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace CensusRx.Services.Attributes;

[AttributeUsage(AttributeTargets.Class), MeansImplicitUse]
public class ServiceLifetimeAttribute : Attribute
{
	public ServiceLifetimeAttribute(ServiceLifetime lifetime)
	{
		Lifetime = lifetime;
	}

	public ServiceLifetime Lifetime { get; }
}