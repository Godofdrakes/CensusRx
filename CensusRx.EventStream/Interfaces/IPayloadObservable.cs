using System.Reactive.Linq;
using DbgCensus.EventStream.Abstractions.Objects;

namespace CensusRx.EventStream;

public interface IPayloadObservable<out TPayload>
	where TPayload : IPayload
{
	public static IPayloadObservable<TPayload> Null { get; } = new NullPayloadObservable<TPayload>();

	IObservable<TPayload> PayloadReceived { get; }
}

internal class NullPayloadObservable<T> : IPayloadObservable<T> where T : IPayload
{
	public IObservable<T> PayloadReceived { get; } = Observable.Never<T>();
}