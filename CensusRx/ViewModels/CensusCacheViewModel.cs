using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CensusRx.Interfaces;
using ReactiveUI;

namespace CensusRx.ViewModels;

public class CensusCacheViewModel<T> : ReactiveObject, ICensusCache<T> where T : ICensusViewModel
{
	private Dictionary<long, BehaviorSubject<T?>> Cache { get; }

	private ReplaySubject<T> ReplaySubject { get; }

	public IObserver<T> Precache => ReplaySubject.AsObserver();

	public CensusCacheViewModel(int cacheSize)
	{
		ReplaySubject = new ReplaySubject<T>(cacheSize, RxApp.MainThreadScheduler);
		Cache = new Dictionary<long, BehaviorSubject<T?>>(cacheSize);
	}

	private BehaviorSubject<T?> GetSubject(long id)
	{
		if (!Cache.TryGetValue(id, out var behaviorSubject))
		{
			behaviorSubject = new BehaviorSubject<T?>(default);
			ReplaySubject
				.Where(value => value.CensusObject.Id == id)
				.Subscribe(behaviorSubject);
			Cache[id] = behaviorSubject;
		}

		return behaviorSubject;
	}

	public IObservable<T> Get(long id) => GetSubject(id).WhereNotNull();
}
