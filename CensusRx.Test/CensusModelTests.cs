using System.Reactive.Linq;
using CensusRx.Test.JSON;

namespace CensusRx.Test;

[TestFixture]
public class CensusModelTests
{
	[Test]
	public void DeserializeCharacter() => CensusJsonData.Character
		.UnwrapCensusCollection<Character>()
		.ToObservable().Test()
		.AssertResultCount(1)
		.AssertValues(Has.None.Null)
		.AssertValues(Has.One.Matches((Character c) => c.Id == 5428016459719850385))
		.AssertValues(Has.One.Matches((Character c) => c.Name.First == "Naozumi"))
		.AssertValues(Has.One.Matches((Character c) => c.Certs.AvailablePoints == 1339));

	[Test]
	public void DeserializeCharacterList() => CensusJsonData.CharacterList
		.UnwrapCensusCollection<Character>()
		.ToObservable().Test()
		.AssertResultCount(4)
		.AssertValues(Has.None.Null);

	[Test]
	public void DeserializeFactionList() => CensusJsonData.FactionList
		.UnwrapCensusCollection<Faction>()
		.ToObservable().Test()
		.AssertResultCount(5)
		.AssertValues(Has.None.Null)
		.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "None"))
		.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "VS"))
		.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "NC"))
		.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "TR"))
		.AssertValues(Has.One.Matches((Faction faction) => faction.CodeTag == "NSO"));
}