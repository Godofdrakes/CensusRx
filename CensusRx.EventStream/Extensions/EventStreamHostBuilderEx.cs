using DbgCensus.EventStream;
using DbgCensus.EventStream.Abstractions.Objects.Control;
using DbgCensus.EventStream.EventHandlers.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CensusRx.EventStream;

public static class EventStreamHostBuilderEx
{
	public static IHostBuilder ConfigureEventStream(this IHostBuilder hostBuilder)
	{
		return hostBuilder.ConfigureServices((context, collection) =>
		{
			collection.Configure<EventStreamOptions>(context.Configuration
				.GetSection(nameof(EventStreamOptions)));

			collection.AddCensusEventHandlingServices()
				.AddPayloadObservable<IHeartbeat>()
				.AddSingleton<IWorldStatusService, WorldStatusService>()
				.AddHostedService<EventStreamWorker>();
		});
	}
}