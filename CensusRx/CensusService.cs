using System.Text;
using RestSharp;

namespace CensusRx;

public interface ICensusService
{
	string Endpoint { get; }
	string ServiceId { get; }
	string Namespace { get; }

	Uri GetEndpointUri()
	{
		var builder = new StringBuilder(Endpoint);

		if (!string.IsNullOrEmpty(ServiceId))
		{
			builder.Append($"/s:{ServiceId}");
		}

		return new Uri(builder.ToString());
	}

	RestRequest CreateGetRequest<T>() => new($"/get/{Namespace}/{typeof(T).Name.ToLower()}");
	RestRequest CreateCountRequest<T>() => new($"/count/{Namespace}/{typeof(T).Name.ToLower()}");
}
