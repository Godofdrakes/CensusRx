using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Splat;

namespace CensusRx.WPF.ViewModels;

public class CensusServiceViewModel : ReactiveObject, ICensusService
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

	[PropertyReference]
	public int Number
	{
		get => _number;
		set => this.RaiseAndSetIfChanged(ref _number, value);
	}

	private int _number;

	public CensusServiceViewModel(IConfiguration configuration, PropertyReferenceRegistry? propertyRegistry = default)
	{
		propertyRegistry ??= Locator.Current.GetService<PropertyReferenceRegistry>();
		propertyRegistry?.RegisterPropertySource(this);

		configuration.Bind("CensusRx", this);
	}
}