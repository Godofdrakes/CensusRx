using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using CensusRx.WPF.Common;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace CensusRx.EventStream.WPF;

public class MainWindowViewModel : WindowViewModel
{
	private readonly ObservableCollectionExtended<IWorldStatusInstance> _worldStatus;

	public IEnumerable<IWorldStatusInstance> WorldStatus => _worldStatus;

	public MainWindowViewModel(IWorldStatusService worldStatusService)
	{
		_worldStatus = new ObservableCollectionExtended<IWorldStatusInstance>();
		worldStatusService.Worlds
			.Connect()
			.ObserveOn(RxApp.MainThreadScheduler)
			.SortBy(status => status.Identifier)
			.Bind(_worldStatus)
			.Subscribe();
	}
}