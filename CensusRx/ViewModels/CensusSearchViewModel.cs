using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
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
	public ReadOnlyObservableCollection<Exception> Errors { get; }

	protected SourceCache<T, long> ResultCache { get; }
	private SourceList<Exception> ErrorCache { get; }

	protected ReactiveCommand<ICensusClient.RequestBuilder<T>, T> ExecuteRequest { get; }

	public CensusSearchViewModel(IScreen? hostScreen = default, ICensusClient? censusClient = default, IScheduler? scheduler = default)
	{
		HostScreen = hostScreen ?? Locator.Current.GetServiceChecked<IScreen>();
		censusClient ??= Locator.Current.GetServiceChecked<ICensusClient>();

		this.ExecuteRequest = ReactiveCommand.CreateFromObservable((ICensusClient.RequestBuilder<T> request) =>
			censusClient.Get(request), outputScheduler: scheduler);

		this.ErrorCache = new SourceList<Exception>();
		this.ResultCache = new SourceCache<T, long>(o => o.Id);

		// Propagate errors
		this.ExecuteRequest.ThrownExceptions
			.Subscribe(exception => this.ErrorCache.Add(exception));
		this.ErrorCache.Connect()
			.Bind(out var errors)
			.Subscribe();
		this.Errors = errors;

		// Propagate results
		this.ExecuteRequest
			.Subscribe(item => this.ResultCache.AddOrUpdate(item));
		this.ResultCache
			.Connect()
			.Bind(out var results)
			.Subscribe();
		this.Results = results;

		var onBeginExecuting = this.ExecuteRequest.IsExecuting
			.Where(isExecuting => isExecuting == true);

		onBeginExecuting.Subscribe(_ => ErrorCache.Clear());
		onBeginExecuting.Subscribe(_ => ResultCache.Clear());
	}

	public string UrlPathSegment => nameof(CharacterSearchViewModel);
	public IScreen HostScreen { get; }
}
