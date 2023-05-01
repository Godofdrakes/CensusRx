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
	private readonly ObservableCollectionExtended<IWorldStatus> _worldStatus;

	public IEnumerable<IWorldStatus> WorldStatus => _worldStatus;

	public MainWindowViewModel(IWorldStatusService worldStatusService)
	{
		_worldStatus = new ObservableCollectionExtended<IWorldStatus>();
		worldStatusService.Worlds
			.Connect()
			.ObserveOn(RxApp.MainThreadScheduler)
			.SortBy(status => status.Identifier)
			.Bind(_worldStatus)
			.Subscribe();
	}
}