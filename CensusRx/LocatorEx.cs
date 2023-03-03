using Splat;

namespace CensusRx;

public static class LocatorEx
{
	public static T GetServiceChecked<T>(
		this IReadonlyDependencyResolver dependencyResolver, string? contract = default) =>
		dependencyResolver.GetService<T>(contract)
		?? throw new InvalidOperationException($"No service registered for type {typeof(T).Name}");
}