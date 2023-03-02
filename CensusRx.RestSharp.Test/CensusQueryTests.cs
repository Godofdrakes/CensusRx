using System.Net;
using RestSharp;
using RichardSzalay.MockHttp;

namespace CensusRx.RestSharp.Test;

[TestFixture]
public class CensusQueryTests
{
	public MockHttpMessageHandler MessageHandler { get; }
	public RestClient RestClient { get; }

	public CensusQueryTests()
	{
		MessageHandler = new MockHttpMessageHandler
		{
			AutoFlush = true,
		};

		RestClient = new RestClient(new RestClientOptions
		{
			BaseUrl = new Uri(CensusJson.GetEndpoint()),
			ConfigureMessageHandler = _ => MessageHandler,
		});
	}

	private void LogResponse(RestRequest request, RestResponse response)
	{
		TestContext.WriteLine($"requestUri		: {RestClient.BuildUri(request)}");
		TestContext.WriteLine($"responseStatus	: {response.ResponseStatus}");
		TestContext.WriteLine($"responseCode	: {response.StatusCode}");
		TestContext.WriteLine($"responseMessage	: {response.ErrorMessage ?? "<none>"}");
	}

	[SetUp]
	public void Setup()
	{
		MessageHandler.Clear();
	}

	[Test]
	public void CharacterTest()
	{
		MessageHandler.Expect($"{CensusJson.GetEndpoint()}/get/ps2/character")
			.WithQueryString("name.first_lower", "naozumi")
			.RespondWithJsonFile(CensusJsonData.CHARACTER);

		var request = CensusQuery.Get()
			.FromNamespace(CensusNamespace.PLANETSIDE_PC)
			.FromCollection(CensusCollections.CHARACTER)
			.Where("name.first_lower", CensusMatch.IsEqualTo("naozumi"));

		var response = RestClient.Execute(request);
		
		LogResponse(request, response);

		Assert.DoesNotThrow(() => MessageHandler.VerifyNoOutstandingExpectation());
		Assert.Multiple(() =>
		{
			Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Completed));
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		});
	}

	[Test]
	public void CharacterListTest()
	{
		MessageHandler.Expect($"{CensusJson.GetEndpoint()}/get/ps2/character")
			.WithQueryString("name.first_lower", "^naozumi")
			.WithQueryString("c:limit", "10")
			.RespondWithJsonFile(CensusJsonData.CHARACTER_LIST);

		var request = CensusQuery.Get()
			.FromCollection(CensusCollections.CHARACTER)
			.FromNamespace(CensusNamespace.PLANETSIDE_PC)
			.Where("name.first_lower", CensusMatch.StartsWith("naozumi"))
			.LimitTo(10);

		var response = RestClient.Execute(request);
		
		LogResponse(request, response);

		Assert.DoesNotThrow(() => MessageHandler.VerifyNoOutstandingExpectation());
		Assert.Multiple(() =>
		{
			Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Completed));
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		});
	}
}
