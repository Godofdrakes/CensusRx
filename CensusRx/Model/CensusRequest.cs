using CensusRx.Interfaces;

namespace CensusRx.Model;

public sealed class CensusRequest<T> : ICensusRequest<T>
	where T : ICensusObject
{
	public static CensusRequest<T> Build(RequestBuilder<T> requestBuilder)
	{
		var request = new CensusRequest<T>();
		requestBuilder.Invoke(request);
		return request;
	}

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

		QueryParams.Add((TempString, censusMatch));
		TempString = null;
		return this;
	}

	public ICensusRequest<T> Bind(Action<(string key, CensusMatch value)> bindAction)
	{
		if (TempString is not null)
			throw new InvalidOperationException("TempString is not null");

		foreach (var pair in this.QueryParams)
			bindAction.Invoke(pair);

		return this;
	}
}
