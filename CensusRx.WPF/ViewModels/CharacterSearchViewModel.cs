using System;
using System.Linq;
using System.Reactive.Linq;
using CensusRx.Model;
using CensusRx.Services;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class CharacterSearchViewModel : CensusSearchViewModel<CharacterViewModel>
{
	public ReactiveCommand<object, string> SearchForCharacters { get; }

	public string Name
	{
		get => _name;
		set => this.RaiseAndSetIfChanged(ref _name, value);
	}

	public CensusMatch? FactionMatch
	{
		get => _factionMatch;
		set => this.RaiseAndSetIfChanged(ref _factionMatch, value);
	}

	private string _name = string.Empty;
	private CensusMatch? _factionMatch;

	public CharacterSearchViewModel(IScreen hostScreen, ICensusClient censusClient, ICensusCache censusCache)
		: base(hostScreen)
	{
		var nameIsValid = this.WhenAnyValue(model => model.Name)
			.Select(name => !string.IsNullOrEmpty(name));

		SearchForCharacters = ReactiveCommand.CreateFromObservable((object _) =>
		{
			return censusClient.Get<Character>(request =>
			{
				request
					.Where(character => character.Name.FirstLower)
					.StartsWith(Name.ToLower())
					.LimitTo(10);

				if (FactionMatch is not null)
				{
					request.Where(character => character.FactionId)
						.Matches(FactionMatch.Value);
				}
			});
		}, nameIsValid);

		SearchForCharacters
			.Select(json => json.UnwrapCensusCollection<Character>()
				.Select(character => new CharacterViewModel(censusCache, character)))
			.Subscribe(characters =>
				ResultCache.Edit(cache =>
					cache.Load(characters)));
	}
}