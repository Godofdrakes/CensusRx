using System.Reactive.Linq;
using CensusRx.RestSharp.Test.JSON;
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
		CensusClient.Get<Character>(_ => { })
			.SelectMany(json => json.UnwrapCensusCollection<Character>())
			.Subscribe(observer);

		observer.AssertSingleException<NotImplementedException>();
	}

	[Test]
	public void RequestCharacter()
	{
		MessageHandler.Expect($"http://localhost/get/ps2/character")
			.WithQueryString("name.first_lower", "naozumi")
			.RespondWithJsonFile(CensusJsonData.CHARACTER);

		var observer = TestCensusObservable(() =>
			CensusClient.Get<Character>(request => request
					.Where(character => character.Name.FirstLower)
					.IsEqualTo("naozumi"))
				.SelectMany(json => json.UnwrapCensusCollection<Character>()));

		observer.AssertResultCount(1);
	}

	[Test]
	public void RequestCharacterList()
	{
		MessageHandler.Expect($"http://localhost/get/ps2/character")
			.WithQueryString("name.first_lower", "^naozumi")
			.WithQueryString("c:limit", "10")
			.RespondWithJsonFile(CensusJsonData.CHARACTER_LIST);

		var observer = TestCensusObservable(() =>
			CensusClient.Get<Character>(request => request
					.Where(character => character.Name.FirstLower)
					.StartsWith("naozumi")
					.LimitTo(10))
				.SelectMany(json => json.UnwrapCensusCollection<Character>()));

		observer.AssertResultCount(4);
	}

	[Test]
	public void RequestCharacterListEmpty()
	{
		MessageHandler.Expect("http://localhost/get/ps2/character")
			.RespondWithJsonFile(CensusJsonData.CHARACTER_LIST_EMPTY);

		var observer = TestCensusObservable(() =>
			CensusClient.Get<Character>(_ => { })
				.SelectMany(json => json.UnwrapCensusCollection<Character>()));

		observer.AssertResultCount(0);
	}
}