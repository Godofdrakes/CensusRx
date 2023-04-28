using DbgCensus.EventStream.Abstractions.Objects;

namespace CensusRx.EventStream;

public interface IPayloadObservable<out TPayload>
	where TPayload : IPayload
{
	IObservable<TPayload> PayloadReceived { get; }
}