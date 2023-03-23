using ReactiveUI;

namespace CensusRx.WPF.Options;

public class ThemeOptions : ReactiveObject
{
	private string? _baseColor;
	private string? _colorScheme;

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
}