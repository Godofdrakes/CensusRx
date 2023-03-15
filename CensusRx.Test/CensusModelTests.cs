using System.Reactive.Linq;
using CensusRx.Test.JSON;

namespace CensusRx.Test;

[TestFixture]
public class CensusModelTests
{
	[Test]
	public void DeserializeCharacter()
	{
		var json = CensusJsonData.GetJsonFile(CensusJsonData.CHARACTER);

		var results = json.UnwrapCensusCollection<Character>()
			.ToObservable()
			.Test();

		results.AssertResultCount(1)
			.AssertValues(Has.None.Null)
			.AssertValues(Has.One.Matches((Character c) => c.Id == 5428016459719850385))
			.AssertValues(Has.One.Matches((Character c) => c.Name.First == "Naozumi"))
			.AssertValues(Has.One.Matches((Character c) => c.Certs.AvailablePoints == 1339));
	}

	[Test]
	public void DeserializeCharacterList()
	{
		var json = CensusJsonData.GetJsonFile(CensusJsonData.CHARACTER_LIST);

		var results = json.UnwrapCensusCollection<Character>()
			.ToObservable()
			.Test();

		results.AssertResultCount(4)
			.AssertValues(Has.None.Null);
	}

	[Test]
	public void DeserializeFactionList()
	{
		var json = CensusJsonData.GetJsonFile(CensusJsonData.FACTION_LIST);

		var results = json.UnwrapCensusCollection<Faction>()
			.ToObservable()
			.Test();

		results.AssertResultCount(5)
			.AssertValues(Has.None.Null)
			.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "None"))
			.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "VS"))
			.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "NC"))
			.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "TR"))
			.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "NSO"));
	}
}