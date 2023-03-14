using System;
using System.Net;
using System.Reactive.Linq;
using CensusRx.Interfaces;
using CensusRx.WPF.Interfaces;
using ReactiveUI;
using RestSharp;

namespace CensusRx.WPF.Services;

public class CensusClient : ReactiveObject, ICensusClient
{
	public Guid Id { get; } = Guid.NewGuid();

	public ICensusService Service { get; }

	public RestClient RestClient { get; }

	public CensusClient(
		ICensusService censusService,
		RestClientOptions? options = default)
	{
		Service = censusService;

		options ??= new RestClientOptions
		{
			ThrowOnAnyError = true,
		};

		options.BaseUrl = Service.GetEndpointUri();

		this.RestClient = new RestClient(options);
	}

	private IObservable<RestResponse> ExecuteRequest(RestRequest restRequest) =>
		Observable.FromAsync(token => RestClient.ExecuteAsync(restRequest, token));

	private static string GetResponseContent(RestResponse restResponse)
	{
		restResponse.ThrowIfError();

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
		var request = Service.CreateGetRequest(requestBuilder);
		return ExecuteRequest(request).Select(GetResponseContent);
	}

	public IObservable<int> Count<T>(RequestBuilder<T> requestBuilder)
		where T : ICensusObject
	{
		throw new NotImplementedException();
	}
}