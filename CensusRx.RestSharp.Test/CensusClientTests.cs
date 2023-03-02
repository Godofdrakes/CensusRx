using RestSharp;
using RichardSzalay.MockHttp;
using TestRx;

namespace CensusRx.RestSharp.Test;

[TestFixture]
public class CensusClientTests : CensusTestsBase
{
	[Test]
	public void RequestWithNoHandlerThrows()
	{
		var observer = new TestObserver<Character>
		{
			AutoLog = true,
		};

		// technically this is testing the mock message handler but it's important this work as expected
		CensusClient.ExecuteRequest<Character>(new RestRequest("/requestWithNoHandler"))
			.Subscribe(observer);

		observer.AssertSingleException<NotImplementedException>();
	}

	[Test]
	public void RequestCharacterList()
	{
		MessageHandler.Expect($"{CensusJson.GetEndpoint()}/get/ps2/character")
			.WithQueryString("name.first_lower", "naozumi")
			.RespondWithJsonFile(CensusJsonData.CHARACTER_LIST);

		var observer = new TestObserver<Character>
		{
			ValueWriter = WriteCensusObjectId,
			AutoLog = true,
		};

		CensusClient.Get<Character>(request => request
				.Where(character => character.Name.FirstLower)
				.IsEqualTo("naozumi"))
			.Subscribe(observer);

		observer.AssertResultCount(4);
	}
}