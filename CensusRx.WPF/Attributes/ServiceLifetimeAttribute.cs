using System;
using Microsoft.Extensions.DependencyInjection;

namespace CensusRx.WPF.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ServiceLifetimeAttribute : Attribute
{
	public ServiceLifetimeAttribute(ServiceLifetime lifetime)
	{
		Lifetime = lifetime;
	}

	public ServiceLifetime Lifetime { get; }
}