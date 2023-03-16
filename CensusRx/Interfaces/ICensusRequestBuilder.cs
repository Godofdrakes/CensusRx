using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace CensusRx.Interfaces;

public delegate void RequestBuilder<T>(ICensusRequestBuilder<T> request) where T : ICensusObject;

internal sealed class PropertyVisitor : ExpressionVisitor
{
	public Stack<MemberInfo> PathStack { get; } = new();

	public JsonNamingPolicy? NamingPolicy { get; set; }

	protected override Expression VisitMember(MemberExpression node)
	{
		if (node.Member is not PropertyInfo propertyInfo)
		{
			throw new InvalidOperationException("Expression must be property chain");
		}

		PathStack.Push(propertyInfo);

		return base.VisitMember(node);
	}

	public override string ToString()
	{
		string ConvertName(MemberInfo info) => NamingPolicy?.ConvertName(info.Name) ?? info.Name;
		return string.Join(".", PathStack.Select(ConvertName));
	}
}

public interface ICensusMatchBuilder<T> where T : ICensusObject
{
	ICensusRequestBuilder<T> Matches(CensusMatch censusMatch);

	ICensusRequestBuilder<T> IsEqualTo(string value) => Matches(CensusMatch.IsEqualTo(value));
	ICensusRequestBuilder<T> StartsWith(string value) => Matches(CensusMatch.StartsWith(value));
}

public interface ICensusRequestBuilder<T> where T : ICensusObject
{
	ICensusMatchBuilder<T> Where(string query);

	ICensusMatchBuilder<T> Where<TProp>(Expression<Func<T, TProp>> expression)
	{
		var visitor = new PropertyVisitor
		{
			NamingPolicy = CensusJson.SerializerOptions.PropertyNamingPolicy
		};

		visitor.Visit(expression.Body);

		return Where(visitor.ToString());
	}

	ICensusRequestBuilder<T> LimitTo(int count) => Where("c:limit").IsEqualTo(count.ToString());
}