using System;
using System.Linq;
using System.Reactive.Linq;
using CensusRx.Interfaces;
using CensusRx.Model;
using CensusRx.Services;
using CensusRx.Services.Interfaces;
using CensusRx.WPF.Interfaces;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class WeaponSearchViewModel : CensusSearchViewModel<Item>
{
	private string _name = string.Empty;
	private CensusMatch? _factionMatch;
	private CensusMatch? _categoryMatch;

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

	public CensusMatch? CategoryMatch
	{
		get => _categoryMatch;
		set => this.RaiseAndSetIfChanged(ref _categoryMatch, value);
	}

	public WeaponSearchViewModel(IScreen hostScreen, ICensusClient censusClient)
		: base(hostScreen, censusClient)
	{ }

	protected override void BuildCensusRequest(ICensusRequest<Item> request)
	{
		request
			.Where(item => item.Name.En)
			.StartsWith(Name)
			.LimitTo(100);

		if (FactionMatch is not null)
		{
			request
				.Where(item => item.FactionId)
				.Matches(FactionMatch.Value);
		}

		if (CategoryMatch is not null)
		{
			request
				.Where(item => item.ItemCategoryId)
				.Matches(CategoryMatch.Value);
		}
	}

	protected override ICensusViewModel<Item> CreateViewModel(Item model)
	{
		return new ItemViewModel(model);
	}
}