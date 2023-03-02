using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CensusRx.JsonConverters;
using CensusRx.Model;
using JorgeSerrano.Json;
using RestSharp;
using RestSharp.Serializers.Json;

namespace CensusRx;

public static class CensusJson
{
	public static JsonSerializerOptions SerializerOptions => new()
	{
		NumberHandling = JsonNumberHandling.AllowReadingFromString,
		PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy(),
	};

	public static SystemTextJsonSerializer Serializer => new(SerializerOptions);

	public static T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, SerializerOptions);

	public static IEnumerable<T> UnwrapCensusCollection<T>(this JsonDocument jsonDocument) =>
		jsonDocument.RootElement.EnumerateObject()
			.First(prop => prop.Value.ValueKind == JsonValueKind.Array)
			.Value.EnumerateArray()
			.Select(element => element.Deserialize<T>(SerializerOptions) ??
			                   throw new InvalidOperationException("element.Deserialize returned null"));

	public static IObservable<T> UnwrapCensusCollection<T>(this IObservable<RestResponse> observable) =>
		observable.SelectMany(response =>
		{
			if (response.Content is null)
				throw new InvalidOperationException("response.Content is null");

			return Observable.Using(
				() => JsonDocument.Parse(response.Content),
				document => document.UnwrapCensusCollection<T>().ToObservable());
		});

	public static string GetEndpoint(string? serviceId = default, CensusFormat format = default)
	{
		var builder = new StringBuilder(CensusConstants.ENDPOINT);

		if (!string.IsNullOrEmpty(serviceId))
		{
			builder.Append($"/s:{serviceId}");
		}

		// requests are assumed to be json
		if (format != CensusFormat.JSON)
		{
			builder.Append($"/{format.ToString().ToLower()}");
		}

		return builder.ToString();
	}
}
