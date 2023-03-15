using System;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using CensusRx.Interfaces;
using CensusRx.WPF.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using RestSharp;

namespace CensusRx.WPF.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
public class CensusClient : ReactiveObject, ICensusClient
{
	public RestClient RestClient { get; }

	public string Endpoint { get; }
	public string ServiceId { get; }
	public string Namespace { get; }

	public CensusClient(IConfiguration configuration)
	{
		var config = configuration.GetRequiredSection("CensusRx");

		Endpoint = config.GetRequiredSection(nameof(Endpoint)).Value!;
		ServiceId = config.GetRequiredSection(nameof(ServiceId)).Value!;
		Namespace = config.GetRequiredSection(nameof(Namespace)).Value!;

		var builder = new StringBuilder(Endpoint);

		if (!string.IsNullOrEmpty(ServiceId))
		{
			builder.Append($"/s:{ServiceId}");
		}

		var options = new RestClientOptions
		{
			BaseUrl = new Uri(builder.ToString()),
			ThrowOnAnyError = true,
		};

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

	RestRequest CreateGetRequest<T>(RequestBuilder<T> requestBuilder) where T : ICensusObject
	{
		var restRequest = new RestRequest($"/get/{Namespace}/{typeof(T).Name.ToLower()}");

		var censusRequest = new CensusRequest<T>();

		requestBuilder.Invoke(censusRequest);

		foreach (var (key, value) in censusRequest.QueryParams)
		{
			restRequest.AddQueryParameter(key, value, false);
		}
		
		return restRequest;
	}

	RestRequest CreateCountRequest<T>() where T : ICensusObject
		=> new($"/count/{Namespace}/{typeof(T).Name.ToLower()}");

	public IObservable<string> Get<T>(RequestBuilder<T> requestBuilder)
		where T : ICensusObject
	{
		var request = CreateGetRequest(requestBuilder);
		return ExecuteRequest(request).Select(GetResponseContent);
	}

	public IObservable<int> Count<T>(RequestBuilder<T> requestBuilder)
		where T : ICensusObject
	{
		throw new NotImplementedException();
	}
}