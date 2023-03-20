using CensusRx.Interfaces;

namespace CensusRx.Test;

[TestFixture]
public class CensusRequestTests
{
	private static CensusRequest<T> MakeRequest<T>(RequestBuilder<T> builder)
		where T : ICensusObject
	{
		var request = new CensusRequest<T>();
		builder.Invoke(request);
		return request;
	}

	[Test]
	public void TestCharacterNameSearch()
	{
		var request = MakeRequest<Character>(request => request
			.Where(character => character.Name.FirstLower)
			.StartsWith("naozumi")
			.LimitTo(10));

		Assert.That(request.QueryParams, Contains
			.Key("name.first_lower")
			.WithValue("^naozumi"));
		Assert.That(request.QueryParams, Contains
			.Key("c:limit")
			.WithValue(10.ToString()));
	}
}