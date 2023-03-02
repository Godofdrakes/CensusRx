using System.Reactive.Linq;
using RestSharp;
using RichardSzalay.MockHttp;

namespace CensusRx.RestSharp.Test;

[TestFixture]
public abstract class CensusTestsBase
{
	public sealed class TestingCensusClient : CensusClient
	{
		public TestingCensusClient(RestClientOptions? options = default)
			: base(CensusNamespace.PLANETSIDE_PC, options: options) { }

		protected override IObservable<RestResponse> ExecuteRequest(RestRequest restRequest)
		{
			// make requests blocking so tests run synchronously
			return Observable.Defer(() => Observable.Return(RestClient.Execute(restRequest)));
		}
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
		this.MessageHandler.Fallback
			.Throw(message =>
				new NotImplementedException($"No handler registered for request: {message.RequestUri}"));

		PerTestSetUp();
	}

	protected virtual void PerTestSetUp()
	{
		
	}

	public static void WriteCensusObjectId(TextWriter writer, ICensusObject censusObject) =>
		writer.WriteLine(censusObject.Id);
}
