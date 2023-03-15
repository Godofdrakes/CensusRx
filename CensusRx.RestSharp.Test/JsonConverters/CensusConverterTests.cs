using System.Reactive.Linq;
using CensusRx.Model.RestSharp.Test.JSON;

namespace CensusRx.RestSharp.Test.JsonConverters;

[TestFixture]
public class CensusConverterTests : CensusTestsBase
{
	[Test]
	public void DeserializeCharacter()
	{
		var json = CensusJsonData.GetJsonFile(CensusJsonData.CHARACTER);

		var observer = TestCensusObservable(() => json.UnwrapCensusCollection<Character>()
			.ToObservable());

		observer.AssertResultCount(1)
			.AssertValues(Has.None.Null)
			.AssertValues(Has.One.Matches((Character c) => c.Id == 5428016459719850385))
			.AssertValues(Has.One.Matches((Character c) => c.Name.First == "Naozumi"))
			.AssertValues(Has.One.Matches((Character c) => c.Certs.AvailablePoints == 1339));
	}

	[Test]
	public void DeserializeCharacterList()
	{
		var json = CensusJsonData.GetJsonFile(CensusJsonData.CHARACTER_LIST);

		var observer = TestCensusObservable(() => json.UnwrapCensusCollection<Character>()
			.ToObservable());

		observer.AssertResultCount(4)
			.AssertValues(Has.None.Null);
	}

	[Test]
	public void DeserializeFactionList()
	{
		var json = CensusJsonData.GetJsonFile(CensusJsonData.FACTION_LIST);

		var observer = TestCensusObservable(() => json.UnwrapCensusCollection<Faction>()
			.ToObservable());

		observer.AssertResultCount(5)
			.AssertValues(Has.None.Null)
			.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "None"))
			.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "VS"))
			.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "NC"))
			.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "TR"))
			.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "NSO"));
	}
}