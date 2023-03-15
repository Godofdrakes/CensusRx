using System;
using System.Collections.ObjectModel;
using CensusRx.Interfaces;
using CensusRx.WPF.Interfaces;
using DynamicData;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public abstract class CensusSearchViewModel<T> : ReactiveObject, IRoutableViewModel
	where T : ICensusViewModel
{
	public string UrlPathSegment => GetType().Name;
	public IScreen HostScreen { get; }

	public ReadOnlyObservableCollection<T> Results => _results;

	protected SourceCache<T, long> ResultCache { get; }

	private readonly ReadOnlyObservableCollection<T> _results;

	protected CensusSearchViewModel(IScreen hostScreen)
	{
		HostScreen = hostScreen;

		ResultCache = new SourceCache<T, long>(o => o.CensusObject.Id);
		ResultCache.Connect().Bind(out _results).Subscribe();
	}
}
