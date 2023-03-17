using System.Reactive.Linq;
using CensusRx.Model;
using ReactiveUI;

namespace CensusRx.WPF.ViewModels;

public class FactionMatch : ReactiveObject
{
	public string Label
	{
		get => _label;
		set => this.RaiseAndSetIfChanged(ref _label, value);
	}

	public FactionId FactionId
	{
		get => _factionId;
		set => this.RaiseAndSetIfChanged(ref _factionId, value);
	}

	public CensusMatch? Match => _match.Value;

	private string _label = string.Empty;
	private FactionId _factionId = FactionId.None;

	private readonly ObservableAsPropertyHelper<CensusMatch?> _match;

	public FactionMatch()
	{
		_match = this.WhenAnyValue(x => x.FactionId)
			.Select(id => (CensusMatch?)(id != FactionId.None ? CensusMatch.IsEqualTo(id) : null))
			.ToProperty(this, x => x.Match);
	}
}