using System;
using System.Threading.Tasks;
using CensusRx.EventStream.WPF.Workers;
using CensusRx.Rest;
using CensusRx.WPF.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CensusRx.EventStream.WPF;

public static class Program
{
	[STAThread]
	public static Task Main(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureSplat()
			.ConfigureWpf<App>()
			.ConfigureServices(collection =>
			{
				collection.AddHostedService<EventStreamWorker>();
			})
			.ConfigureRest()
			.ConfigureEventStream()
			.Build()
			.ConfigureSplat()
			.RunAsync();
}