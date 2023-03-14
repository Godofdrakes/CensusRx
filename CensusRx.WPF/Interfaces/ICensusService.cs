using System;
using System.Text;
using CensusRx.Interfaces;
using CensusRx.Model;
using RestSharp;

namespace CensusRx.WPF.Interfaces;

public interface ICensusService
{
	string Endpoint { get; }
	string ServiceId { get; }
	string Namespace { get; }

	Uri GetEndpointUri()
	{
		var builder = new StringBuilder(Endpoint);

		if (!string.IsNullOrEmpty(ServiceId))
		{
			builder.Append($"/s:{ServiceId}");
		}

		return new Uri(builder.ToString());
	}

	RestRequest CreateGetRequest<T>(RequestBuilder<T> requestBuilder) where T : ICensusObject
	{
		var restRequest = new RestRequest($"/get/{Namespace}/{typeof(T).Name.ToLower()}");
		var censusRequest = new CensusRequest<T>();
		requestBuilder.Invoke(censusRequest);
		censusRequest.Bind(tuple => restRequest.AddQueryParameter(tuple.key, tuple.value, false));
		return restRequest;
	}

	RestRequest CreateCountRequest<T>() where T : ICensusObject
		=> new($"/count/{Namespace}/{typeof(T).Name.ToLower()}");
}
