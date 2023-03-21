using System.Reactive.Linq;
using CensusRx.Interfaces;
using CensusRx.Model;
using CensusRx.Services;
using CensusRx.WPF.Interfaces;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class CharacterSearchViewModel : CensusSearchViewModel<Character>
{
	private readonly ICensusCache _censusCache;

	private string _name = string.Empty;
	private CensusMatch? _factionMatch;

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

	public CharacterSearchViewModel(IScreen hostScreen, ICensusClient censusClient, ICensusCache censusCache)
		: base(hostScreen, censusClient)
	{
		_censusCache = censusCache;

		var nameIsValid = this.WhenAnyValue(model => model.Name)
			.Select(name => !string.IsNullOrEmpty(name));

		nameIsValid.Subscribe(CanSearch);
	}

	protected override void BuildCensusRequest(ICensusRequest<Character> request)
	{
		request
			.Where(character => character.Name.FirstLower)
			.StartsWith(Name.ToLower())
			.LimitTo(100);

		if (FactionMatch is not null)
		{
			request.Where(character => character.FactionId)
				.Matches(FactionMatch.Value);
		}
	}

	protected override ICensusViewModel<Character> CreateViewModel(Character model)
	{
		return new CharacterViewModel(_censusCache, model);
	}
}