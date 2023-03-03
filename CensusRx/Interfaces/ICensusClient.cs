namespace CensusRx.Interfaces;

public interface ICensusClient
{
	IObservable<string> Get<T>(RequestBuilder<T> requestBuilder) where T : ICensusObject;
	IObservable<int> Count<T>(RequestBuilder<T> requestBuilder) where T : ICensusObject;
}
