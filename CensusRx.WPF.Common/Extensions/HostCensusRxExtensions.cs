using Microsoft.Extensions.Hosting;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace CensusRx.WPF.Common;

public static class HostCensusRxExtensions
{
	public static IHost ConfigureSplat(this IHost host)
	{
		host.Services.UseMicrosoftDependencyResolver();
		return host;
	}
}