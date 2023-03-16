using System;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CensusRx.Interfaces;
using CensusRx.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using RestSharp;

namespace CensusRx.WPF.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
public class CensusClient : ReactiveObject, ICensusClient
{
	private RestClient RestClient { get; }

	public string Endpoint { get; }
	public string ServiceId { get; }
	public string Namespace { get; }

	public CensusClient(IConfiguration configuration, IHostApplicationLifetime applicationLifetime)
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

		applicationLifetime.ApplicationStopping.Register(() => this.RestClient.Dispose());
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

	private RestRequest CreateGetRequest<T>(RequestBuilder<T> requestBuilder) where T : ICensusObject
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

	private RestRequest CreateCountRequest<T>() where T : ICensusObject
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

	public Task StartAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		RestClient.Dispose();
		return Task.CompletedTask;
	}
}