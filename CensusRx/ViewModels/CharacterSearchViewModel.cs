using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using CensusRx.Interfaces;
using CensusRx.Model;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;

namespace CensusRx.ViewModels;

public class CharacterSearchViewModel : CensusSearchViewModel<Character>
{
	public ReactiveCommand<string, Unit> NameSearch { get; }

	public string Name
	{
		get => _name;
		set => this.RaiseAndSetIfChanged(ref _name, value);
	}

	private string _name = string.Empty;

	public CharacterSearchViewModel(IScreen? hostScreen = default, ICensusClient? censusClient = default, IScheduler? scheduler = default)
		: base(hostScreen, censusClient, scheduler)
	{
		var nameIsValid = this.WhenAnyValue(model => model.Name)
			.Select(name => !string.IsNullOrEmpty(name));

		var isExecuting = this.ExecuteRequest.IsExecuting;

		var canExecute = nameIsValid.CombineLatest(
			isExecuting,
			(valid, executing) => valid && !executing);

		this.ValidationRule(
			model => model.Name,
			nameIsValid,
			"You must specify a valid name");

		NameSearch = ReactiveCommand.CreateFromObservable((string name) => this.ExecuteRequest.Execute(request =>
					request
						.LimitTo(10)
						.Where(character => character.Name.FirstLower)
						.StartsWith(name.ToLower()))
				.Select(_ => Unit.Default),
			canExecute);
	}
}
