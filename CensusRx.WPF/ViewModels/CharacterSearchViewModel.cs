using System;
using System.Linq;
using System.Reactive.Linq;
using CensusRx.Model;
using CensusRx.Services;
using CensusRx.WPF.Interfaces;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class CharacterSearchViewModel : CensusSearchViewModel<CharacterViewModel>
{
	public ReactiveCommand<object, string> NameSearch { get; }

	public string Name
	{
		get => _name;
		set => this.RaiseAndSetIfChanged(ref _name, value);
	}

	private string _name = string.Empty;

	public CharacterSearchViewModel(IScreen hostScreen, ICensusClient censusClient)
		: base(hostScreen)
	{
		var nameIsValid = this.WhenAnyValue(model => model.Name)
			.Select(name => !string.IsNullOrEmpty(name));
		
		NameSearch = ReactiveCommand.CreateFromObservable((object _) =>
		{
			return censusClient.Get<Character>(request => request
				.Where(character => character.Name.FirstLower)
				.StartsWith(Name.ToLower())
				.LimitTo(10));
		}, nameIsValid);
		
		NameSearch
			.Select(json => json.UnwrapCensusCollection<Character>())
			.Subscribe(characters =>
				ResultCache.Edit(cache =>
					cache.Load(characters.Select(character => new CharacterViewModel(character)))));
	}
}