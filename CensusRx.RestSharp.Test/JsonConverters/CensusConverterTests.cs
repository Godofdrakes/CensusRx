using System.Text.Json;
using CensusRx.Model;

namespace CensusRx.RestSharp.Test.JsonConverters;

[TestFixture]
public class CensusConverterTests
{
	public static JsonDocument ParseJsonFile(string file) => JsonDocument.Parse(CensusJsonData.GetJsonFile(file));

	[Test]
	public void DeserializeCharacter()
	{
		using var document = ParseJsonFile(CensusJsonData.CHARACTER);

		var characters = document.UnwrapCensusCollection<Character>().ToList();

		Assert.That(characters, Is.Not.Null);
		Assert.That(characters, Is.Not.Empty);
		Assert.That(characters, Has.Count.EqualTo(1));

		var character = characters!.FirstOrDefault();

		Assert.That(character, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(character!.CharacterId, Is.Not.EqualTo(0));
			Assert.That(character!.Name, Is.Not.EqualTo(Character.CharacterName.Invalid));
			Assert.That(character!.Certs, Is.Not.EqualTo(Character.CharacterCerts.Zero));
		});
	}

	[Test]
	public void DeserializeCharacterList()
	{
		using var document = ParseJsonFile(CensusJsonData.CHARACTER_LIST);

		var characters = document.UnwrapCensusCollection<Character>().ToList();

		Assert.That(characters, Is.Not.Null);
		Assert.That(characters, Is.Not.Empty);
		Assert.That(characters, Has.Count.EqualTo(4));
	}

	[Test]
	public void DeserializeFactionList()
	{
		using var document = ParseJsonFile(CensusJsonData.FACTION_LIST);

		var factions = document.UnwrapCensusCollection<Faction>().ToList();

		Assert.That(factions, Is.Not.Null);
		Assert.That(factions, Is.Not.Empty);
		Assert.That(factions, Has.Count.EqualTo(5));
		Assert.That(factions, Has.One.Matches((Faction faction) => faction.CodeTag == "None"));
		Assert.That(factions, Has.One.Matches((Faction faction) => faction.CodeTag == "VS"));
		Assert.That(factions, Has.One.Matches((Faction faction) => faction.CodeTag == "NC"));
		Assert.That(factions, Has.One.Matches((Faction faction) => faction.CodeTag == "TR"));
		Assert.That(factions, Has.One.Matches((Faction faction) => faction.CodeTag == "NSO"));
	}
}