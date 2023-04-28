using System.Reactive.Linq;
using System.Reactive.Subjects;
using DbgCensus.EventStream.Abstractions.Objects;
using DbgCensus.EventStream.EventHandlers.Abstractions;

namespace CensusRx.EventStream;

internal class ObservablePayloadHandler<T> : IPayloadHandler<T>, IPayloadObservable<T>
	where T : IPayload
{
	private readonly Subject<T> _payloadReceived = new();

	public IObservable<T> PayloadReceived => _payloadReceived
		.AsObservable();

	public Task HandleAsync(T payload, CancellationToken ct = default)
	{
		_payloadReceived.OnNext(payload);
		return Task.CompletedTask;
	}
}