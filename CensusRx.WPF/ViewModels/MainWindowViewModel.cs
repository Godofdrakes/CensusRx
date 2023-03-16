using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CensusRx.Services;
using CensusRx.WPF.Interfaces;
using DynamicData;

namespace CensusRx.WPF.ViewModels;

public class MainWindowViewModel : WindowViewModel
{
	public CharacterSearchViewModel CharacterSearch { get; }
	public CharacterSearchViewModel CharacterSearch2 { get; }

	public MainWindowViewModel(ICensusClient censusClient)
	{
		CharacterSearch = new CharacterSearchViewModel(this, censusClient);
		CharacterSearch2 = new CharacterSearchViewModel(this, censusClient);
	}
}