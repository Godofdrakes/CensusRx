using System.IO;
using System.Reflection;
using CensusRx.Interfaces;
using CensusRx.WPF.Interfaces;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Splat;

namespace CensusRx.WPF.ServiceModules;

public class CensusServiceModule : ReactiveObject, IServiceModule, ICensusService
{
	private string _endpoint = string.Empty;
	private string _serviceId = string.Empty;
	private string _namespace = string.Empty;

	[PropertyReference]
	public string Endpoint
	{
		get => _endpoint;
		set => this.RaiseAndSetIfChanged(ref _endpoint, value);
	}

	[PropertyReference]
	public string ServiceId
	{
		get => _serviceId;
		set => this.RaiseAndSetIfChanged(ref _serviceId, value);
	}

	[PropertyReference]
	public string Namespace
	{
		get => _namespace;
		set => this.RaiseAndSetIfChanged(ref _namespace, value);
	}

	public CensusClient CensusClient { get; }

	public CensusServiceModule()
	{
		// todo only use user secrets in dev builds?
		var config = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", false, false)
			.AddUserSecrets(Assembly.GetExecutingAssembly())
			.Build();

		config.Bind("CensusRx", this);

		CensusClient = new CensusClient(this);
	}

	public void Register(IMutableDependencyResolver locator)
	{
		locator.RegisterConstant<ICensusService>(this);
		locator.RegisterConstant<ICensusClient>(this.CensusClient);
	}

	public void Startup(IReadonlyDependencyResolver locator)
	{
		if (locator.TryGetService<PropertyReferenceRegistry>(out var propertyRegistry))
		{
			propertyRegistry.RegisterPropertySource(this);
		}
	}
}