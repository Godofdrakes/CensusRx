using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveUI.Testing;
using RichardSzalay.MockHttp;
using TestRx;

namespace CensusRx.Util.Test;

[TestFixture]
public class ResourceCacheTests
{
	public MockHttpMessageHandler MessageHandler { get; }

	public HttpClient HttpClient { get; }

	public ResourceCache<string> Cache { get; }

	public ResourceCacheTests()
	{
		this.MessageHandler = new MockHttpMessageHandler();

		this.HttpClient = new HttpClient(this.MessageHandler);

		this.Cache = new ResourceCache<string>(10)
		{
			HttpClient = this.HttpClient,
			ContentHandler = content => Observable.FromAsync(() => content.ReadAsStringAsync()),
		};
	}

	[SetUp]
	public void Setup()
	{
		MessageHandler.ResetExpectations();
		MessageHandler.ResetBackendDefinitions();
		MessageHandler.Fallback.Throw(new InvalidOperationException("No matching mock for request"));

		Cache.InvalidateAll();
	}

	private void TestGetInternal(string uri, string text) => new TestScheduler().With(scheduler =>
	{
		var observer = new TestObserver<string>();

		TestContext.WriteLine($"uri: {uri}");
		
		var observable = Cache.Get(new Uri(uri));
		
		observable.Subscribe(observer);

		Assert.DoesNotThrow(() =>
		{
			scheduler.Start();

			// todo: eliminate the need for this
			// ideally this test would be single-threaded but the web request is async
			observable.Wait();

			MessageHandler.VerifyNoOutstandingExpectation();
		    MessageHandler.VerifyNoOutstandingRequest();
	    });

	    observer.AssertResults(text);
	});

	[Test]
	public void GetCached()
	{
		const string fullUri = $"http://localhost/get/foo";

		MessageHandler.Expect(fullUri).Respond("text/plain", "bar");

		// get once as normal
		TestGetInternal(fullUri, "bar");

		// get again, which should just return the cached value
		TestGetInternal(fullUri, "bar");
	}
}
