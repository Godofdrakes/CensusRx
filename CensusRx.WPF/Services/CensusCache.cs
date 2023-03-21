using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using CensusRx.Interfaces;
using CensusRx.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CensusRx.WPF.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
public class CensusCache : ICensusCache
{
	private record CensusKey(Type Type, long Id)
	{
		public CensusKey(ICensusObject censusObject) : this(censusObject.GetType(), censusObject.Id) {}
	}

	private Dictionary<CensusKey, ReplaySubject<ICensusObject>> Subjects { get; } = new();

	private ReplaySubject<ICensusObject> GetSubject(CensusKey key)
	{
		if (Subjects.TryGetValue(key, out var subject)) return subject;
		return Subjects[key] = new ReplaySubject<ICensusObject>(1);
	}

	public IObservable<ICensusObject> Get(Type type, long id) => GetSubject(new CensusKey(type, id));

	public void Precache(IObservable<ICensusObject> observable)
	{
		observable.Subscribe(censusObject =>
		{
			GetSubject(new CensusKey(censusObject)).OnNext(censusObject);
		});
	}
}