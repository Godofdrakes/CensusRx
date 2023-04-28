using System;
using System.Threading.Tasks;
using CensusRx.WPF.Common;
using Microsoft.Extensions.Hosting;

namespace CensusRx.EventStream.WPF;

public static class Program
{
	[STAThread]
	public static Task Main(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureSplat()
			.ConfigureWpf<App>()
			.ConfigureEventStream()
			.Build()
			.ConfigureSplat()
			.RunAsync();
}