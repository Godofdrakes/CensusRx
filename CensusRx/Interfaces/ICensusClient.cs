namespace CensusRx.Interfaces;

public interface ICensusClient
{
	IObservable<string> Get<T>(ICensusRequest<T>.RequestBuilder requestBuilder) where T : ICensusObject;
	IObservable<int> Count<T>(ICensusRequest<T>.RequestBuilder requestBuilder) where T : ICensusObject;
}
