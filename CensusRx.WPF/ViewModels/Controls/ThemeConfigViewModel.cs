using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows;
using CensusRx.Services;
using CensusRx.WPF.Interfaces;
using CensusRx.WPF.Options;
using ControlzEx.Theming;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
		get => _baseColor;
		set => this.RaiseAndSetIfChanged(ref _baseColor, value);
	}

	public string? ColorScheme
	{
		get => _colorScheme;
		set => this.RaiseAndSetIfChanged(ref _colorScheme, value);
	}

	public ReactiveCommand<Unit,Unit> ApplyChanges { get; }

	private string? _baseColor;
	private string? _colorScheme;

	public ThemeConfigViewModel(IScreen hostScreen, Application application, IServiceProvider serviceProvider)
	{
		var themeManager = serviceProvider.GetRequiredService<ThemeManager>();
		var themeOptions = serviceProvider.GetRequiredService<IOptions<ThemeOptions>>();

		HostScreen = hostScreen;

		AllBaseColors = themeManager.BaseColors;
		AllColorSchemes = themeManager.ColorSchemes;

		BaseColor = themeOptions.Value.BaseColor;
		ColorScheme = themeOptions.Value.ColorScheme;

		var theme = themeManager.DetectTheme();
		if (theme is not null)
		{
			BaseColor ??= theme.BaseColorScheme;
			ColorScheme ??= theme.ColorScheme;
		}

		ApplyChanges = ReactiveCommand.Create(() =>
		{
			themeManager.ChangeTheme(application, BaseColor!, ColorScheme!);

			serviceProvider.TryGetService<IOptionsWriter<ThemeOptions>>(writer =>
			{
				writer.Write(options =>
				{
					options.BaseColor = BaseColor;
					options.ColorScheme = ColorScheme;
				});
			});
		});
	}
}