using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CensusRx.Interfaces;
using CensusRx.Services.Interfaces;
using CensusRx.WPF.Interfaces;
using DynamicData;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public abstract class CensusSearchViewModel<T> : ReactiveObject, IRoutableViewModel
	where T : ICensusObject
{
	public string UrlPathSegment => GetType().Name;
	public IScreen HostScreen { get; }

	public ReactiveCommand<object,string> Search { get; }

	protected BehaviorSubject<bool> CanSearch { get; } = new(true);

	public ReadOnlyObservableCollection<ICensusViewModel<T>> Results => _results;

	private readonly ReadOnlyObservableCollection<ICensusViewModel<T>> _results;
	private readonly SourceCache<ICensusViewModel<T>, long> _resultCache;

	protected CensusSearchViewModel(IScreen hostScreen, ICensusClient censusClient)
	{
		HostScreen = hostScreen;

		_resultCache = new SourceCache<ICensusViewModel<T>, long>(o => o.CensusObject.Id);
		_resultCache.Connect().Bind(out _results).Subscribe();

		IObservable<string> Get(object sender)
		{
			return censusClient.Get<T>(BuildCensusRequest);
		}

		IEnumerable<ICensusViewModel<T>> Deserialize(string json)
		{
			return json.UnwrapCensusCollection<T>().Select(CreateViewModel);
		}

		void Add(IEnumerable<ICensusViewModel<T>> viewModels)
		{
			_resultCache.Edit(cache =>
			{
				cache.Load(viewModels);
			});
		}

		Search = ReactiveCommand.CreateFromObservable<object,string>(Get, CanSearch);
		Search.Select(Deserialize).Subscribe(Add);
	}

	protected abstract void BuildCensusRequest(ICensusRequest<T> request);
	protected abstract ICensusViewModel<T> CreateViewModel(T model);
}
