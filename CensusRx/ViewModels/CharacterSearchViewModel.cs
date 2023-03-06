using System.Reactive.Linq;
using CensusRx.Interfaces;
using CensusRx.Model;
using ReactiveUI;
using Splat;

namespace CensusRx.ViewModels;

public class CharacterSearchViewModel : CensusSearchViewModel<CharacterViewModel>
{
	public ReactiveCommand<string, string> NameSearch { get; }

	public string Name
	{
		get => _name;
		set => this.RaiseAndSetIfChanged(ref _name, value);
	}

	private string _name = string.Empty;

	public CharacterSearchViewModel(IScreen? hostScreen = default, ICensusClient? censusClient = default)
		: base(hostScreen)
	{
		censusClient ??= Locator.Current.GetServiceChecked<ICensusClient>();

		var nameIsValid = this.WhenAnyValue(model => model.Name)
			.Select(name => !string.IsNullOrEmpty(name));

		NameSearch = ReactiveCommand.CreateFromObservable((string name) =>
		{
			return censusClient.Get<Character>(request => request
				.Where(character => character.Name.FirstLower)
				.StartsWith(name.ToLower())
				.LimitTo(10));
		}, nameIsValid);

		NameSearch
			.Select(json => json.UnwrapCensusCollection<Character>())
			.Subscribe(characters =>
				ResultCache.Edit(cache =>
					cache.Load(characters.Select(character => new CharacterViewModel(character)))));
	}
}
