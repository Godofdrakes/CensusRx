using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using CensusRx.WPF.Common;
using DynamicData;
using ReactiveUI;

namespace CensusRx.EventStream.WPF;

public class MainWindowViewModel : WindowViewModel, IActivatableViewModel
{
	public ViewModelActivator Activator { get; } = new();
}