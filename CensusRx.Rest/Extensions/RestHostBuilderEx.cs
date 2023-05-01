using DbgCensus.Rest;
using DbgCensus.Rest.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CensusRx.Rest;

public static class RestHostBuilderEx
{
	public static IHostBuilder ConfigureRest(this IHostBuilder hostBuilder)
	{
		return hostBuilder.ConfigureServices((context, collection) =>
		{
			collection.Configure<CensusQueryOptions>(context.Configuration
				.GetSection(nameof(CensusQueryOptions)));

			collection.AddCensusRestServices();
		});
	}
}