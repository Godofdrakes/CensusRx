using CensusRx.Interfaces;
using CensusRx.Services.Attributes;
using Microsoft.Extensions.Hosting;

namespace CensusRx.Services.Interfaces;

[ServiceInterface]
public interface ICensusClient : IHostedService
{
	IObservable<string> Get<T>(RequestBuilder<T> requestBuilder) where T : ICensusObject;
	IObservable<int> Count<T>(RequestBuilder<T> requestBuilder) where T : ICensusObject;
}
