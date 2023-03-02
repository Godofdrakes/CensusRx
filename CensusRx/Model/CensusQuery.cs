using RestSharp;

namespace CensusRx.Model;

public static class CensusQuery
{
	private const string Verb = nameof(Verb);
	private const string Namespace = nameof(Namespace);
	private const string Collection = nameof(Collection);

	private const string QueryFormat = $"{{{Verb}}}/{{{Namespace}}}/{{{Collection}}}";

	public static RestRequest Get() => new RestRequest(QueryFormat)
		.AddUrlSegment(Verb, nameof(CensusVerb.GET).ToLower());

	public static RestRequest Count() => new RestRequest(QueryFormat)
		.AddUrlSegment(Verb, nameof(CensusVerb.COUNT).ToLower());

	public static RestRequest FromNamespace(this RestRequest request, string @namespace) =>
		request.AddUrlSegment(Namespace, @namespace);

	public static RestRequest FromCollection(this RestRequest request, string collection) =>
		request.AddUrlSegment(Collection, collection);

	public static RestRequest LimitTo(this RestRequest request, int count) =>
		request.AddQueryParameter("c:limit", count, false);

	public static RestRequest Where(this RestRequest request, string key, CensusMatch match) =>
		request.AddQueryParameter(key, match.ToString(), false);
}
