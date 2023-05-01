using System.Reactive.Linq;
using System.Reactive.Subjects;
using DbgCensus.EventStream.Abstractions.Objects;
using DbgCensus.EventStream.EventHandlers.Abstractions;
using Microsoft.Extensions.Logging;

namespace CensusRx.EventStream;

internal sealed class ObservablePayloadHandler<T> : IPayloadHandler<T>, IPayloadObservable<T>
	where T : IPayload
{
	private readonly ILogger<ObservablePayloadHandler<T>> _logger;
	private readonly Subject<T> _payloadReceived = new();

	public IObservable<T> PayloadReceived => _payloadReceived
		.AsObservable();

	public ObservablePayloadHandler(ILogger<ObservablePayloadHandler<T>> logger)
	{
		_logger = logger;
	}
	
	public Task HandleAsync(T payload, CancellationToken ct = default)
	{
		_logger.LogDebug("{PayloadType} received", typeof(T).Name);
		_payloadReceived.OnNext(payload);
		return Task.CompletedTask;
	}
}