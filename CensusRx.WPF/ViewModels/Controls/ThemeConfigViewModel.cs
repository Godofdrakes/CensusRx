using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using ControlzEx.Theming;
using Microsoft.Extensions.Configuration;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class ThemeConfigViewModel : ReactiveObject, IRoutableViewModel
{
	public string UrlPathSegment { get; } = nameof(ThemeConfigViewModel);
	public IScreen HostScreen { get; }

	public ReadOnlyObservableCollection<string> AllBaseColors { get; }
	public ReadOnlyObservableCollection<string> AllColorSchemes { get; }

	public string? BaseColor
	{
		get => Configuration[nameof(BaseColor)];
		set
		{
			this.RaisePropertyChanging();
			Configuration[nameof(BaseColor)] = value;
			this.RaisePropertyChanged();
		}
	}

	public string? ColorScheme
	{
		get => Configuration[nameof(ColorScheme)];
		set
		{
			this.RaisePropertyChanging();
			Configuration[nameof(ColorScheme)] = value;
			this.RaisePropertyChanged();
		}
	}

	private IConfiguration Configuration { get; }

	public ThemeConfigViewModel(
		IScreen hostScreen,
		Application application,
		ThemeManager themeManager,
		IConfiguration configuration)
	{
		HostScreen = hostScreen;
		Configuration = configuration.GetSection("theme");

		AllBaseColors = themeManager.BaseColors;
		AllColorSchemes = themeManager.ColorSchemes;

		var theme = themeManager.DetectTheme();
		if (theme is not null)
		{
			BaseColor ??= theme.BaseColorScheme;
			ColorScheme ??= theme.ColorScheme;
		}

		// todo must write custom code to allow saving

		this.WhenAnyValue(model => model.BaseColor)
			.WhereNotNull()
			.Subscribe(baseColor => themeManager.ChangeThemeBaseColor(application, baseColor));
		this.WhenAnyValue(model => model.ColorScheme)
			.WhereNotNull()
			.Subscribe(colorScheme => themeManager.ChangeThemeColorScheme(application, colorScheme));
	}
}