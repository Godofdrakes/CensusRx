using CensusRx.Interfaces;
using CensusRx.Model;
using CensusRx.Services;
using CensusRx.WPF.Interfaces;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class WeaponSearchViewModel : CensusSearchViewModel<Item>
{
	private string _name = string.Empty;
	private string? _factionMatch;
	private string? _categoryMatch;

	public string Name
	{
		get => _name;
		set => this.RaiseAndSetIfChanged(ref _name, value);
	}

	public string? FactionMatch
	{
		get => _factionMatch;
		set => this.RaiseAndSetIfChanged(ref _factionMatch, value);
	}

	public string? CategoryMatch
	{
		get => _categoryMatch;
		set => this.RaiseAndSetIfChanged(ref _categoryMatch, value);
	}

	public WeaponSearchViewModel(IScreen hostScreen, ICensusClient censusClient)
		: base(hostScreen, censusClient) { }

	protected override void BuildCensusRequest(ICensusRequest<Item> request)
	{
		if (!string.IsNullOrEmpty(Name))
		{
			request.Where(item => item.Name.En).StartsWith(Name);
		}

		if (FactionMatch is not null)
		{
			request.Where(item => item.FactionId).IsEqualTo(FactionMatch);
		}

		if (CategoryMatch is not null)
		{
			request.Where(item => item.ItemCategoryId).IsEqualTo(CategoryMatch);
		}

		request.Join(item => item.WeaponDatasheet)
			.Join(item => item.Weapon)
			.CaseSensitive(false)
			.LimitTo(100);
	}

	protected override ICensusViewModel<Item> CreateViewModel(Item model)
	{
		return new ItemViewModel(model);
	}
}