using System.Text;
using CensusRx.Interfaces;

namespace CensusRx;

public sealed class CensusRequest<T> : ICensusRequest<T>
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

		public ICensusRequest<T> Matches(CensusMatch censusMatch)
		{
			CensusRequest.QueryParams.Add(Query, censusMatch);
			return CensusRequest;
		}
	}

	public class JoinBuilder : ICensusJoinBuilder<T>
	{
		public class JoinArgs
		{
			public string Type { get; set; }
			public string InsertAt { get; set; }
		}

		public CensusRequest<T> CensusRequest { get; }

		public JoinBuilder(CensusRequest<T> censusRequest)
		{
			CensusRequest = censusRequest;
		}

		public List<JoinArgs> Args { get; } = new();

		public ICensusJoinBuilder<T> Insert(string type, string insertAt)
		{
			Args.Add(new JoinArgs
			{
				Type = type,
				InsertAt = insertAt
			});

			return this;
		}
	}

	// todo: make this string,string
	public Dictionary<string, CensusMatch> QueryParams { get; } = new();

	public ICensusRequest<T> Join(JoinBuilder<T> joinBuilder)
	{
		throw new NotImplementedException();
	}

	public ICensusMatchBuilder<T> Where(string query) => new MatchBuilder(this, query);
}