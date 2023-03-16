using CensusRx.Interfaces;

namespace CensusRx;

public sealed class CensusRequest<T> : ICensusRequestBuilder<T>
	where T : ICensusObject
{
	private class MatchBuilder : ICensusMatchBuilder<T>
	{
		public CensusRequest<T> CensusRequest { get; }

		public string Query { get; }

		public MatchBuilder(CensusRequest<T> censusRequest, string query)
		{
			CensusRequest = censusRequest;
			Query = query;
		}

		public ICensusRequestBuilder<T> Matches(CensusMatch censusMatch)
		{
			CensusRequest.QueryParams.Add(Query, censusMatch);
			return CensusRequest;
		}
	}

	public Dictionary<string, CensusMatch> QueryParams { get; } = new();

	public ICensusMatchBuilder<T> Where(string query) => new MatchBuilder(this, query);
}