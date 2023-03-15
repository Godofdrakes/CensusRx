using JetBrains.Annotations;

namespace Microsoft.Extensions.DependencyInjection;

[AttributeUsage(AttributeTargets.Class), MeansImplicitUse]
public class ServiceLifetimeAttribute : Attribute
{
	public ServiceLifetimeAttribute(ServiceLifetime lifetime)
	{
		Lifetime = lifetime;
	}

	public ServiceLifetime Lifetime { get; }
}