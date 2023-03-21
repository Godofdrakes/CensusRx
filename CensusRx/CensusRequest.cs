using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using CensusRx.Interfaces;

namespace CensusRx;

public record CensusJoinArgs(string Type, string InjectAt);

public sealed class CensusRequest<T> : ICensusRequest<T>
	where T : ICensusObject
{
	private sealed class MatchBuilder : ICensusMatchBuilder<T>
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

	private sealed class PropertyVisitor : ExpressionVisitor
	{
		public Stack<MemberInfo> PathStack { get; } = new();

		public override Expression? Visit(Expression? node)
		{
			if (node is MemberExpression { Member: PropertyInfo })
			{
				return base.Visit(node);
			}

			if (node is ParameterExpression)
			{
				return base.Visit(node);
			}

			throw new InvalidOperationException("Expression must be property chain");
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			PathStack.Push(node.Member);
			return base.VisitMember(node);
		}

		private IEnumerable<string> GetPath()
		{
			return PathStack.Select(info => info.Name);
		}

		public override string ToString()
		{
			return ToString(namingPolicy: null);
		}

		public string ToString(JsonNamingPolicy? namingPolicy)
		{
			var path = GetPath();

			if (namingPolicy is not null)
			{
				path = path.Select(namingPolicy.ConvertName);
			}

			return string.Join(".", path);
		}
	}

	public JsonNamingPolicy? NamingPolicy { get; set; }

	// todo: make this string,string ?
	public Dictionary<string, CensusMatch> QueryParams { get; } = new();

	public List<CensusJoinArgs> JoinArgs { get; } = new();

	public ICensusMatchBuilder<T> Where(string query)
	{
		return new MatchBuilder(this, query);
	}

	public ICensusRequest<T> Join(string type, string injectAt)
	{
		JoinArgs.Add(new CensusJoinArgs(type, injectAt));
		return this;
	}

	public ICensusMatchBuilder<T> Where<TProp>(Expression<Func<T, TProp>> expression)
	{
		var visitor = new PropertyVisitor();

		visitor.Visit(expression.Body);

		return Where(visitor.ToString(NamingPolicy));
	}

	public ICensusRequest<T> Join<TProp>(Expression<Func<T, TProp?>> expression) where TProp : ICensusObject
	{
		var visitor = new PropertyVisitor();

		visitor.Visit(expression.Body);

		var type = typeof(TProp).Name;

		if (this.NamingPolicy is not null)
		{
			type = NamingPolicy.ConvertName(type);
		}

		return Join(type, visitor.ToString(NamingPolicy));
	}
}