using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Splat;

namespace CensusRx.Util;

public class ResourceCache<T>
{
	public delegate IObservable<T> ContentHandlerFunc(HttpContent content);

	public HttpClient? HttpClient { get; set; }

	public ContentHandlerFunc? ContentHandler { get; set; }

	private MemoizingMRUCache<Uri, IObservable<T>> Cache { get; }

	public ResourceCache(int maxSize)
	{
		this.Cache = new MemoizingMRUCache<Uri, IObservable<T>>((uri, _) =>
		{
			var httpClient = GetChecked(this.HttpClient);
			var contentHandler = GetChecked(this.ContentHandler);

			var observable = Observable.Using<HttpContent,HttpResponseMessage>(
				token => httpClient.GetAsync(uri, token),
				(message, _) => Task.FromResult(Observable.Return(message.EnsureSuccessStatusCode().Content)))
				.SelectMany(contentHandler.Invoke)
				.Replay(1);
			observable.Connect();
			return observable;
		}, maxSize);
	}

	private static TProp GetChecked<TProp>([NotNull] TProp? property, [CallerArgumentExpression("property")] string propertyName = "")
	{
		if (property is null)
		{
			throw new InvalidOperationException($"{propertyName} is null");
		}

		return property;
	}
	
	public IObservable<T> Get(Uri key) => Cache.Get(key);

	public bool TryGet(Uri key, out IObservable<T>? result) => Cache.TryGet(key, out result);

	public void Invalidate(Uri key) => Cache.Invalidate(key);

	public void InvalidateAll(bool aggregateReleaseExceptions = false) =>
		Cache.InvalidateAll(aggregateReleaseExceptions);

	public IEnumerable<IObservable<T>> CachedValues() => Cache.CachedValues();
}