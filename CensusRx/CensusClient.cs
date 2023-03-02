using System.Reactive.Linq;
using CensusRx.Interfaces;
using CensusRx.Model;
using RestSharp;
using Splat;

namespace CensusRx;

public class CensusClient : ICensusClient
{
	public ICensusService Service { get; }

	public RestClient RestClient { get; }

	public CensusClient(
		ICensusService? censusService = default,
		RestClientOptions? options = default)
	{
		Service = censusService ?? Locator.Current.GetServiceChecked<ICensusService>();

		options ??= new RestClientOptions
		{
			ThrowOnDeserializationError = true,
			ThrowOnAnyError = true,
		};

		options.BaseUrl = Service.GetEndpoint();

		this.RestClient = new RestClient(options).UseSerializer(() => CensusJson.Serializer);
	}

	protected virtual IObservable<RestResponse> ExecuteRequest(RestRequest restRequest) =>
		Observable.FromAsync(token => RestClient.ExecuteAsync(restRequest, token));

	public IObservable<T> ExecuteRequest<T>(RestRequest request) =>
		ExecuteRequest(request).UnwrapCensusCollection<T>();

	public IObservable<T> Get<T>(ICensusClient.RequestBuilder<T> requestBuilder)
		where T : ICensusObject
	{
		var restRequest = Service.CreateGetRequest<T>().Build(requestBuilder);
		return ExecuteRequest(restRequest).UnwrapCensusCollection<T>();
	}

	public IObservable<int> Count<T>(ICensusClient.RequestBuilder<T> requestBuilder)
		where T : ICensusObject
	{
		throw new NotImplementedException();
	}
}