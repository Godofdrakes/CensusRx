using CensusRx.Interfaces;
using RestSharp;

namespace CensusRx.Model;

public static class CensusRequest
{
	public static RestRequest Build<T>(this RestRequest restRequest, ICensusClient.RequestBuilder<T> requestBuilder)
		where T : ICensusObject
	{
		var request = new CensusRequest<T>();
		requestBuilder.Invoke(request);
		request.Bind(restRequest);
		return restRequest;
	}
}

public sealed class CensusRequest<T> : ICensusRequest<T>
	where T : ICensusObject
{
	private string? TempString { get; set; }

	private List<(string key, CensusMatch value)> QueryParams { get; } = new();

	public ICensusRequest<T> Where(string query)
	{
		if (TempString is not null)
			throw new InvalidOperationException("TempString is not null");

		TempString = query;
		return this;
	}

	public ICensusRequest<T> Matches(CensusMatch censusMatch)
	{
		if (TempString is null)
			throw new InvalidOperationException("TempString is null");

		QueryParams.Add(new (TempString, censusMatch));
		TempString = null;
		return this;
	}

	public void Bind(RestRequest restRequest)
	{
		if (TempString is not null)
			throw new InvalidOperationException("TempString is not null");

		foreach (var (key, value) in this.QueryParams)
			restRequest.AddQueryParameter(key, value, false);
	}
}
