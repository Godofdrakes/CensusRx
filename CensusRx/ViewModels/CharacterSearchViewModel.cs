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

	public CharacterSearchViewModel(IScreen? hostScreen = default, ICensusClient? censusClient = default)
		: base(hostScreen, censusClient)
	{
		var nameIsValid = this.WhenAnyValue(model => model.Name)
			.Select(name => !string.IsNullOrEmpty(name));

		var canExecute = nameIsValid.CombineLatest(
			ExecuteRequest.IsExecuting,
			(valid, executing) => valid && !executing);

		NameSearch = ReactiveCommand.CreateFromObservable((string name) =>
				ExecuteRequest.Execute(request => request
						.Where(character => character.Name.FirstLower)
						.StartsWith(name.ToLower())
						.LimitTo(10))
					.Select(_ => Unit.Default),
			canExecute);
	}
}
