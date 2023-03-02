using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveUI.Testing;
using RichardSzalay.MockHttp;
using TestRx;

namespace CensusRx.Util.Test;

[TestFixture, Ignore("not implemented")]
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
		var observer = new TestObserver<string>
		{
			AutoLog = true,
		};

		TestContext.WriteLine($"uri: {uri}");

		Cache.Get(new Uri(uri)).Subscribe(observer);

		Assert.DoesNotThrow(() =>
		{
			scheduler.Start();
		    MessageHandler.VerifyNoOutstandingExpectation();
		    MessageHandler.VerifyNoOutstandingRequest();
	    });

	    observer.AssertResults(text);
	});

    [TestCase("foo")]
	public void TestGet(string text)
	{
		var fullUri = $"http://localhost/get/{text}";

		MessageHandler.Expect(fullUri).Respond("text/plain", text);

		TestGetInternal(fullUri, text);
	}

	[TestCase("foo")]
	public void TestGetCached(string text)
	{
		var fullUri = $"http://localhost/get/{text}";

		MessageHandler.Expect(fullUri).Respond("text/plain", text);

		// get once as normal
		TestGetInternal(fullUri, text);

		// get again, which should just return the cached value
		TestGetInternal(fullUri, text);
	}
}
