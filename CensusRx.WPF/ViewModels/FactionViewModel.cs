﻿using CensusRx.Interfaces;
using CensusRx.Model;
using CensusRx.WPF.Interfaces;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class FactionViewModel : ReactiveObject, ICensusViewModel<Faction>
{
	public Faction CensusObject
	{
		get => _censusObject;
		set => this.RaiseAndSetIfChanged(ref _censusObject, value);
	}

	private Faction _censusObject;

	public FactionViewModel(Faction? faction = default)
	{
		_censusObject = faction ?? new Faction();
	}
}