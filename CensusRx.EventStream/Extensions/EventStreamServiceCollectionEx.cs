using DbgCensus.EventStream.Abstractions.Objects;
using DbgCensus.EventStream.EventHandlers.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CensusRx.EventStream;

public static class EventStreamServiceCollectionEx
{
	public static IServiceCollection AddPayloadObservable<T>(this IServiceCollection services)
		where T : IPayload
	{
		services.AddSingleton<ObservablePayloadHandler<T>>();
		services.AddTransient<IPayloadHandler<T>>(GetPayloadObservable);
		services.AddTransient<IPayloadObservable<T>>(GetPayloadObservable);
		return services;

		ObservablePayloadHandler<T> GetPayloadObservable(IServiceProvider provider)
		{
			return provider.GetRequiredService<ObservablePayloadHandler<T>>();
		}
	}
}