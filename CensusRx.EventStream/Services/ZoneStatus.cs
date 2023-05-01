using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;

namespace CensusRx.EventStream;

internal sealed class ZoneStatus : ReactiveObject, IZoneStatus, IDisposable
{
	public ZoneIdentifier Identifier { get; }
	public bool IsUnlocked => _isUnlocked.Value;

	private readonly ObservableAsPropertyHelper<bool> _isUnlocked;

	private readonly CompositeDisposable _disposable = new();

	public ZoneStatus(ZoneIdentifier identifier)
	{
		Identifier = identifier;

		_isUnlocked = Observable.Return(true)
			.ToProperty(this, status => status.IsUnlocked, deferSubscription: true);
	}

	public void Dispose() => _disposable.Dispose();
}