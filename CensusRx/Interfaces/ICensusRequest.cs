using System.Linq.Expressions;
using System.Text.Json;

namespace CensusRx.Interfaces;

public delegate void RequestBuilder<T>(ICensusRequest<T> request) where T : ICensusObject;

public interface ICensusMatchBuilder<T> where T : ICensusObject
{
	ICensusRequest<T> Matches(string match);

	ICensusRequest<T> IsEqualTo(string value) => Matches(CensusMatch.IsEqualTo(value));
	ICensusRequest<T> IsEqualTo(object value, JsonSerializerOptions? serializerOptions) => Matches(CensusMatch.IsEqualTo(value, serializerOptions));
	ICensusRequest<T> StartsWith(string value) => Matches(CensusMatch.StartsWith(value));
	ICensusRequest<T> Contains(string value) => Matches(CensusMatch.Contains(value));
}

public interface ICensusRequest<T> where T : ICensusObject
{
	ICensusMatchBuilder<T> Where(string query);
	ICensusMatchBuilder<T> Where<TProp>(Expression<Func<T, TProp>> expression);

	ICensusRequest<T> Join(string type, string injectAt);
	ICensusRequest<T> Join<TProp>(Expression<Func<T, TProp?>> expression) where TProp : ICensusObject;

	ICensusRequest<T> LimitTo(int count) => Where("c:limit").IsEqualTo(count, default);

	// Makes string matching not case sensitive. Is slower than matching against name.lower (if available).
	ICensusRequest<T> CaseSensitive(bool value) => Where("c:case").IsEqualTo(value, default);
}