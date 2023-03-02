namespace CensusRx.Interfaces;

public interface ICensusClient
{
	delegate void RequestBuilder<T>(ICensusRequest<T> request) where T : ICensusObject;

	IObservable<T> Get<T>(RequestBuilder<T> requestBuilder) where T : ICensusObject;
	IObservable<int> Count<T>(RequestBuilder<T> requestBuilder) where T : ICensusObject;
}
