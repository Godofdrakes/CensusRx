using CensusRx.Interfaces;
using Microsoft.Extensions.Hosting;

namespace CensusRx.Services;

[ServiceInterface]
public interface ICensusClient : IHostedService
{
	IObservable<string> Get<T>(RequestBuilder<T> requestBuilder) where T : ICensusObject;
	IObservable<int> Count<T>(RequestBuilder<T> requestBuilder) where T : ICensusObject;

	IObservable<Uri> LastRequest { get; }
}
