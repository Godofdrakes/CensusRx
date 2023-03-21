using System.Reactive.Linq;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class FactionMatch : ReactiveObject
{
	public string Label
	{
		get => _label;
		set => this.RaiseAndSetIfChanged(ref _label, value);
	}

	public long? FactionId
	{
		get => _factionId;
		set => this.RaiseAndSetIfChanged(ref _factionId, value);
	}

	public string? Match => _match.Value;

	private string _label = string.Empty;
	private long? _factionId;

	private readonly ObservableAsPropertyHelper<string?> _match;

	public FactionMatch()
	{
		string? CreateFactionMatch(long? factionId)
		{
			if (factionId is not null)
			{
				return CensusMatch.IsEqualTo(factionId.Value, CensusJson.SerializerOptions);
			}

			return null;
		}

		_match = this.WhenAnyValue(x => x.FactionId)
			.Select(CreateFactionMatch)
			.ToProperty(this, x => x.Match);
	}
}