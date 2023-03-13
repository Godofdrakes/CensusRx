using System.Net;
using System.Reactive.Linq;
using CensusRx.Interfaces;
using CensusRx.Model;
using RestSharp;

namespace CensusRx;

public class CensusClient : ICensusClient
{
	public ICensusService Service { get; }

	public RestClient RestClient { get; }

	public CensusClient(
		ICensusService censusService,
		RestClientOptions? options = default)
	{
		Service = censusService;

		options ??= new RestClientOptions
		{
			ThrowOnDeserializationError = true,
			ThrowOnAnyError = true,
		};

		options.BaseUrl = Service.GetEndpointUri();

		this.RestClient = new RestClient(options).UseSerializer(() => CensusJson.Serializer);
	}

	protected virtual IObservable<RestResponse> ExecuteRequest(RestRequest restRequest) =>
		Observable.FromAsync(token => RestClient.ExecuteAsync(restRequest, token));

	private static string GetResponseContent(RestResponse restResponse)
	{
		if (restResponse.StatusCode != HttpStatusCode.OK)
		{
			throw new InvalidOperationException("restResponse.StatusCode != HttpStatusCode.OK");
		}

		if (restResponse.Content is null)
		{
			throw new InvalidOperationException("restResponse.Content is null");
		}

		return restResponse.Content;
	}

	public IObservable<string> Get<T>(RequestBuilder<T> requestBuilder)
		where T : ICensusObject
	{
		var censusRequest = CensusRequest<T>.Build(requestBuilder);
		var restRequest = Service.CreateGetRequest<T>().Bind(censusRequest);
		return ExecuteRequest(restRequest).Select(GetResponseContent);
	}

	public IObservable<int> Count<T>(RequestBuilder<T> requestBuilder)
		where T : ICensusObject
	{
		throw new NotImplementedException();
	}
}