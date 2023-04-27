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
	private readonly ObservableCollectionExtended<WorldStatus> _worldStatus;

	public IEnumerable<WorldStatus> WorldStatus => _worldStatus;

	public MainWindowViewModel(IWorldStatusService worldStatusService)
	{
		_worldStatus = new ObservableCollectionExtended<WorldStatus>();

		worldStatusService.WorldStatusCache
			.Connect()
			.ObserveOn(RxApp.MainThreadScheduler)
			.Bind(_worldStatus)
			.Subscribe();
	}
}