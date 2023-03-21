using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace CensusRx.Interfaces;

public delegate void RequestBuilder<T>(ICensusRequest<T> request) where T : ICensusObject;
public delegate void JoinBuilder<T>(ICensusJoinBuilder<T> builder) where T : ICensusObject;

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
	ICensusRequest<T> Matches(CensusMatch censusMatch);

	ICensusRequest<T> IsEqualTo(string value) => Matches(CensusMatch.IsEqualTo(value));
	ICensusRequest<T> IsEqualTo(object value) => Matches(CensusMatch.IsEqualTo(value));
	ICensusRequest<T> StartsWith(string value) => Matches(CensusMatch.StartsWith(value));
	ICensusRequest<T> Contains(string value) => Matches(CensusMatch.Contains(value));
}

public interface ICensusJoinBuilder<T> where T : ICensusObject
{
	ICensusJoinBuilder<T> Insert(string type, string insertAt);
	ICensusJoinBuilder<T> Insert<TProp>(Expression<Func<T, TProp?>> expression) where TProp : ICensusObject
	{
		var visitor = new PropertyVisitor
		{
			NamingPolicy = CensusJson.SerializerOptions.PropertyNamingPolicy
		};

		visitor.Visit(expression.Body);

		return Insert(typeof(TProp).Name, visitor.ToString());
	}
}

public interface ICensusRequest<T> where T : ICensusObject
{
	ICensusRequest<T> Join(JoinBuilder<T> joinBuilder);
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

	ICensusRequest<T> LimitTo(int count) => Where("c:limit").IsEqualTo(count);

	// Makes string matching not case sensitive. Is slower than matching against name.lower (if available).
	ICensusRequest<T> CaseSensitive(bool value) => Where("c:case").IsEqualTo(value);
}