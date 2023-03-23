using Microsoft.Extensions.DependencyInjection;

namespace CensusRx.Services;

public static class ServiceProviderEx
{
	public static void TryGetService<T>(this IServiceProvider serviceProvider, Action<T> serviceAction)
	{
		var service = serviceProvider.GetService<T>();
		if (service is not null)
		{
			serviceAction.Invoke(service);
		}
	}
}