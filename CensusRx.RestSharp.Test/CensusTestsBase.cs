using System.Reactive.Linq;
using ReactiveUI;
using RestSharp;
using RichardSzalay.MockHttp;
using Splat;
using TestRx;

namespace CensusRx.RestSharp.Test;

[TestFixture]
public abstract class CensusTestsBase
{
	public sealed class TestingCensusService : ReactiveObject, ICensusService
	{
		public string Endpoint => "http://localhost";
		public string ServiceId => string.Empty;
		public string Namespace => CensusNamespace.PLANETSIDE_PC;
	}

	public sealed class TestingCensusClient : CensusClient
	{
		public TestingCensusClient(RestClientOptions? options = default)
			: base(new TestingCensusService(), options: options) { }

		protected override IObservable<RestResponse> ExecuteRequest(RestRequest restRequest) =>
			// make requests blocking so tests run synchronously
			Observable.Defer(() => Observable.Return(RestClient.Execute(restRequest)));
	}

	public MockHttpMessageHandler MessageHandler { get; }
	public TestingCensusClient CensusClient { get; }

	protected CensusTestsBase()
	{
		this.MessageHandler = new MockHttpMessageHandler();

		var restOptions = new RestClientOptions
		{
			ConfigureMessageHandler = _ => this.MessageHandler,
			ThrowOnDeserializationError = true,
			ThrowOnAnyError = true,
		};

		this.CensusClient = new TestingCensusClient(options: restOptions);
	}

	[SetUp]
	public void SetUp()
	{
		this.MessageHandler.Clear();
		this.MessageHandler.Fallback.Throw(message => new NotImplementedException(
			$"No handler registered for request: {message.RequestUri}"));

		var locator = new ModernDependencyResolver();

		locator.RegisterConstant<HttpMessageHandler>(MessageHandler);
		locator.RegisterConstant<ICensusService>(CensusClient.Service);
		locator.RegisterConstant<ICensusClient>(CensusClient);

		PerTestSetUp(locator);

		Locator.SetLocator(locator);
	}

	protected virtual void PerTestSetUp(IMutableDependencyResolver dependencyResolver) { }

	public static TestObserver<T> TestCensusObservable<T>(Func<IObservable<T>> builder)
		where T : ICensusObject
	{
		var observer = new TestObserver<T>
		{
			ValueWriter = (writer, value) => writer.WriteLine(value.Id),
			AutoLog = true,
		};

		builder.Invoke().Subscribe(observer);
		return observer;
	}

	public static TestObserver<T> TestObservable<T>(Func<IObservable<T>> builder)
	{
		var observer = new TestObserver<T>
		{
			AutoLog = true,
		};

		builder.Invoke().Subscribe(observer);
		return observer;
	}
}
