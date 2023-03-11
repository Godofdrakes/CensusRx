using ReactiveUI;
using Splat;

namespace CensusRx.WPF.ViewModels;

public class CensusSettingsViewModel : ReactiveObject, IRoutableViewModel
{
	public string UrlPathSegment { get; } = nameof(CensusSettingsViewModel);
	public IScreen HostScreen { get; }

	public PropertyReferenceRegistry PropertyRegistry { get; }

	public CensusSettingsViewModel(IScreen? hostScreen = default, PropertyReferenceRegistry? propertyRegistry = default)
	{
		HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>()!;
		PropertyRegistry = propertyRegistry ?? Locator.Current.GetService<PropertyReferenceRegistry>()!;
	}
}