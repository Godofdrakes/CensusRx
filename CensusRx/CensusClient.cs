using System.Reactive.Linq;
using CensusRx.Interfaces;
using CensusRx.Model;
using RestSharp;

namespace CensusRx;

public class CensusClient : ICensusClient
{
	public RestClient RestClient { get; }

	public string Namespace { get; }

	public CensusClient(
		string @namespace,
		string? serviceId = default,
		RestClientOptions? options = default)
	{
		options ??= new RestClientOptions
		{
			ThrowOnDeserializationError = true,
			ThrowOnAnyError = true,
		};

		options.BaseUrl = new Uri(CensusJson.GetEndpoint(serviceId));

		this.RestClient = new RestClient(options).UseSerializer(() => CensusJson.Serializer);
		this.Namespace = @namespace;
	}

	protected virtual IObservable<RestResponse> ExecuteRequest(RestRequest restRequest) =>
		Observable.FromAsync(token => RestClient.ExecuteAsync(restRequest, token));

	public IObservable<T> ExecuteRequest<T>(RestRequest request) =>
		ExecuteRequest(request).UnwrapCensusCollection<T>();

	public IObservable<T> Get<T>(ICensusClient.RequestBuilder<T> requestBuilder) where T : ICensusObject
	{
		var restRequest = new RestRequest($"/get/{Namespace}/{typeof(T).Name.ToLower()}");
		CensusRequest<T>.Build(requestBuilder)
			.ForEachParam(param => restRequest.AddQueryParameter(param.key, param.value, false));
		return ExecuteRequest(restRequest).UnwrapCensusCollection<T>();
	}

	public IObservable<int> Count<T>(ICensusClient.RequestBuilder<T> requestBuilder) where T : ICensusObject
	{
		throw new NotImplementedException();
	}
}
