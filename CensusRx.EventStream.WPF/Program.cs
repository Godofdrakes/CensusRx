using System;
using System.Threading.Tasks;
using CensusRx.WPF.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CensusRx.EventStream.WPF;

public static class Program
{
	[STAThread]
	public static Task Main(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureLogging(builder => builder.AddConsole())
			.ConfigureSplat()
			.ConfigureWpf<App>()
			.Build()
			.ConfigureSplat()
			.RunAsync();
}