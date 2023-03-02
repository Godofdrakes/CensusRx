using CensusRx.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class CensusServiceViewModel : ReactiveObject, ICensusService
{
	private string _endpoint = string.Empty;
	private string _serviceId = string.Empty;
	private string _namespace = string.Empty;

	public string Endpoint
	{
		get => _endpoint;
		set => this.RaiseAndSetIfChanged(ref _endpoint, value);
	}

	public string ServiceId
	{
		get => _serviceId;
		set => this.RaiseAndSetIfChanged(ref _serviceId, value);
	}

	public string Namespace
	{
		get => _namespace;
		set => this.RaiseAndSetIfChanged(ref _namespace, value);
	}

	public CensusServiceViewModel(IConfiguration configuration)
	{
		var configSection = configuration.GetSection("CensusRx");
		ChangeToken.OnChange(configSection.GetReloadToken,
			() => configSection.Bind(this));
		configSection.Bind(this);
	}
}