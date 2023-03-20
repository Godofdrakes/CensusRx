using CensusRx.Interfaces;
using CensusRx.Model;
using CensusRx.WPF.Interfaces;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class ItemViewModel : ReactiveObject, ICensusViewModel<Item>
{
	public Item CensusObject
	{
		get => _censusObject;
		set => this.RaiseAndSetIfChanged(ref _censusObject, value);
	}

	private Item _censusObject;

	public ItemViewModel(Item? item = default)
	{
		_censusObject = item ?? new Item();
	}
}