using System.Collections.ObjectModel;
using System.Reactive.Linq;
using CensusRx.Interfaces;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Validation.Helpers;
using Splat;

namespace CensusRx.ViewModels;

public class CensusSearchViewModel<T> : ReactiveValidationObject, IRoutableViewModel
	where T : ICensusObject
{
	public ReadOnlyObservableCollection<T> Results { get; }

	protected SourceCache<T, long> ResultCache { get; }

	protected ReactiveCommand<ICensusRequest<T>.RequestBuilder, string> ExecuteRequest { get; }

	public CensusSearchViewModel(IScreen? hostScreen = default, ICensusClient? censusClient = default)
	{
		HostScreen = hostScreen ?? Locator.Current.GetServiceChecked<IScreen>();
		censusClient ??= Locator.Current.GetServiceChecked<ICensusClient>();

		this.ExecuteRequest = ReactiveCommand.CreateFromObservable(
			(ICensusRequest<T>.RequestBuilder request) => censusClient.Get(request));

		this.ResultCache = new SourceCache<T, long>(o => o.Id);

		// Propagate results
		this.ExecuteRequest.IsExecuting
			.Where(isExecuting => isExecuting == true)
			.Subscribe(_ => ResultCache.Clear());
		this.ExecuteRequest
			.SelectMany(json => json.UnwrapCensusCollection<T>())
			.Subscribe(ResultCache.AddOrUpdate);
		this.ResultCache
			.Connect()
			.Bind(out var results)
			.Subscribe();
		this.Results = results;
	}

	public string UrlPathSegment => GetType().Name;
	public IScreen HostScreen { get; }
}
