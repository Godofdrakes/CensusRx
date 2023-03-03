using System.Collections.ObjectModel;
using CensusRx.Interfaces;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Validation.Helpers;
using Splat;

namespace CensusRx.ViewModels;

public abstract class CensusSearchViewModel<T> : ReactiveValidationObject, IRoutableViewModel
	where T : ICensusViewModel
{
	public ReadOnlyObservableCollection<T> Results { get; }

	protected SourceCache<T, long> ResultCache { get; }

	public CensusSearchViewModel(IScreen? hostScreen = default)
	{
		HostScreen = hostScreen ?? Locator.Current.GetServiceChecked<IScreen>();

		this.ResultCache = new SourceCache<T, long>(o => o.CensusObject.Id);
		this.ResultCache
			.Connect()
			.Bind(out var results)
			.Subscribe();
		this.Results = results;
	}

	public string UrlPathSegment => GetType().Name;
	public IScreen HostScreen { get; }
}
