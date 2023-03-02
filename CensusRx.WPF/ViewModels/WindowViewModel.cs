using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class WindowViewModel : ReactiveObject, IScreen
{
	public RoutingState Router { get; } = new();

	public ReactiveCommand<IRoutableViewModel, Unit> AdvanceViewModel { get; }
	public ReactiveCommand<IRoutableViewModel, Unit> ResetViewModel { get; }
	public ReactiveCommand<Unit, Unit> RegressViewModel { get; }

	public string Title
	{
		get => _title;
		set => this.RaiseAndSetIfChanged(ref _title, value);
	}

	private string _title;

	protected WindowViewModel()
	{
		var canRegressViewModel = this.WhenAnyValue(model => model.Router.NavigationStack.Count)
			.Select(count => count > 0);
		AdvanceViewModel = ReactiveCommand.CreateFromObservable((IRoutableViewModel model) =>
			Router.Navigate.Execute(model).Select(_ => Unit.Default));
		ResetViewModel = ReactiveCommand.CreateFromObservable((IRoutableViewModel model) =>
			Router.NavigateAndReset.Execute(model).Select(_ => Unit.Default));
		RegressViewModel = ReactiveCommand.CreateFromObservable((Unit _) =>
				Router.NavigateBack.Execute().Select(_ => Unit.Default),
			canRegressViewModel);

		this._title = GetType().Assembly.FullName ?? GetType().Name;
	}
}
