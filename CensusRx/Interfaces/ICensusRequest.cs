using System.Linq.Expressions;
using System.Reflection;
using CensusRx.Model;
using RestSharp;

namespace CensusRx.Interfaces;

public interface ICensusRequest<T> where T : ICensusObject
{
	delegate void RequestBuilder(ICensusRequest<T> request);

	ICensusRequest<T> Where(string query);
	ICensusRequest<T> Matches(CensusMatch censusMatch);
	ICensusRequest<T> Bind(Action<(string key, CensusMatch value)> bindAction);
}

public static class CensusRequest
{
	private class PropertyVisitor : ExpressionVisitor
	{
		public Stack<MemberInfo> PathStack { get; } = new();

		protected override Expression VisitMember(MemberExpression node)
		{
			if (node.Member is not PropertyInfo propertyInfo)
			{
				throw new InvalidOperationException("Expression must be property chain");
			}

			this.PathStack.Push(propertyInfo);
			return base.VisitMember(node);
		}
	}

	public static ICensusRequest<TValue> Where<TValue,TProperty>(this ICensusRequest<TValue> censusRequest, Expression<Func<TValue,TProperty>> expression)
		where TValue : ICensusObject
	{
		var namingPolicy = CensusJson.SerializerOptions.PropertyNamingPolicy;

		var visitor = new PropertyVisitor();
		visitor.Visit(expression.Body);
		return censusRequest.Where(string.Join(".", visitor.PathStack
			.Select(info => namingPolicy?.ConvertName(info.Name) ?? info.Name)));
	}

	public static RestRequest Bind<T>(this RestRequest restRequest, ICensusRequest<T> censusRequest)
		where T : ICensusObject
	{
		censusRequest.Bind(tuple => restRequest.AddQueryParameter(tuple.key, tuple.value, false));
		return restRequest;
	}

	public static ICensusRequest<T> IsEqualTo<T>(this ICensusRequest<T> censusRequest, string value)
		where T : ICensusObject
	{
		return censusRequest.Matches(CensusMatch.IsEqualTo(value));
	}

	public static ICensusRequest<T> StartsWith<T>(this ICensusRequest<T> censusRequest, string value)
		where T : ICensusObject
	{
		return censusRequest.Matches(CensusMatch.StartsWith(value));
	}

	public static ICensusRequest<T> LimitTo<T>(this ICensusRequest<T> censusRequest, int count)
		where T : ICensusObject
	{
		return censusRequest.Where("c:limit").Matches(CensusMatch.IsEqualTo(count));
	}
}
