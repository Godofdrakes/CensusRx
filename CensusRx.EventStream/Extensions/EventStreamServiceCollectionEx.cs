using DbgCensus.EventStream.Abstractions.Objects;
using DbgCensus.EventStream.EventHandlers.Abstractions;
using DbgCensus.EventStream.EventHandlers.Extensions;
using DbgCensus.EventStream.EventHandlers.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CensusRx.EventStream;

public static class EventStreamServiceCollectionEx
{
	public static IServiceCollection AddPayloadObservable<T>(this IServiceCollection services)
		where T : IPayload
	{
		services.AddSingleton<ObservablePayloadHandler<T>>();
		services.AddScoped<IPayloadHandler<T>>(GetPayloadObservable);
		services.AddScoped<IPayloadObservable<T>>(GetPayloadObservable);
		services.Configure<PayloadHandlerTypeRepository>(r => r.RegisterHandler<ObservablePayloadHandler<T>>());

		return services;

		ObservablePayloadHandler<T> GetPayloadObservable(IServiceProvider provider)
		{
			return provider.GetRequiredService<ObservablePayloadHandler<T>>();
		}
	}
}