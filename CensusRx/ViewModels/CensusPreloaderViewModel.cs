using System.Reactive.Concurrency;
using CensusRx.Interfaces;
using ReactiveUI;

namespace CensusRx.ViewModels;

public class CensusPreloaderViewModel : ReactiveObject, IRoutableViewModel, IActivatableViewModel
{
	public string? UrlPathSegment => nameof(CensusPreloaderViewModel);

	public IScreen HostScreen { get; }

	public ViewModelActivator Activator { get; } = new();

	public CensusPreloaderViewModel(ICensusClient? censusClient = default, IScheduler? scheduler = default)
	{
		
	}
}