using System.Text;
using CensusRx.Model;
using ReactiveUI;
using RestSharp;

namespace CensusRx;

public interface ICensusService :
	IReactiveNotifyPropertyChanged<IReactiveObject>,
	IHandleObservableErrors,
	IReactiveObject
{
	string Endpoint { get; }
	string ServiceId { get; }
	string Namespace { get; }
}

public static class CensusService
{
	public static Uri GetEndpoint(this ICensusService censusService, CensusFormat format = CensusFormat.JSON)
	{
		var builder = new StringBuilder(censusService.Endpoint);

		if (!string.IsNullOrEmpty(censusService.ServiceId))
		{
			builder.Append($"/s:{censusService.ServiceId}");
		}

		// requests are assumed to be json
		if (format != CensusFormat.JSON)
		{
			builder.Append($"/{format.ToString().ToLower()}");
		}

		return new Uri(builder.ToString());
	}

	public static RestRequest CreateGetRequest<T>(this ICensusService censusService) =>
		new($"/get/{censusService.Namespace}/{typeof(T).Name.ToLower()}");

	public static RestRequest CreateCountRequest<T>(this ICensusService censusService) =>
		new($"/count/{censusService.Namespace}/{typeof(T).Name.ToLower()}");
}
