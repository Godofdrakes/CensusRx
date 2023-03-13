using CensusRx.WPF.ServiceModules;
using ReactiveUI;
using Splat;

namespace CensusRx.WPF.ViewModels;

public class CensusSettingsViewModel : ReactiveObject, IRoutableViewModel
{
	public string UrlPathSegment { get; } = nameof(CensusSettingsViewModel);
	public IScreen HostScreen { get; }

	public PropertyReferenceRegistry PropertyRegistry { get; }

	public CensusSettingsViewModel(IScreen hostScreen, PropertyReferenceRegistry? propertyRegistry = default)
	{
		HostScreen = hostScreen;
		PropertyRegistry = propertyRegistry ?? Locator.Current.GetService<PropertyReferenceRegistry>()!;
	}
}