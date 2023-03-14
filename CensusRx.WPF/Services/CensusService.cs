using System;
using CensusRx.WPF.Interfaces;
using Microsoft.Extensions.Configuration;
using ReactiveUI;

namespace CensusRx.WPF.Services;

public class CensusService : ReactiveObject, ICensusService
{
	public Guid Id { get; } = Guid.NewGuid();

	[PropertyReference(isReadOnly: true)]
	public string Endpoint
	{
		get => _endpoint;
		set => this.RaiseAndSetIfChanged(ref _endpoint, value);
	}

	[PropertyReference(isReadOnly: true)]
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

	private string _endpoint = string.Empty;
	private string _serviceId = string.Empty;
	private string _namespace = string.Empty;
	
	public CensusService(IConfiguration configuration, PropertyReferenceRegistry propertyRegistry)
	{
		configuration.Bind("CensusRx", this);
		propertyRegistry.RegisterPropertySource(this);
	}
}