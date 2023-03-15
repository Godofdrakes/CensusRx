using TestRx;

namespace CensusRx.Test;

public static class ObservableEx
{
	public static TestObserver<T> Test<T>(this IObservable<T> observable)
	{
		var observer = new TestObserver<T>();
		observable.Subscribe(observer);
		return observer;
	}
}