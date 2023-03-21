using System.Reactive.Linq;

namespace CensusRx.Test;

[TestFixture]
public class CensusModelTests
{
	[Test]
	public void DeserializeCharacterList() => CensusJsonData.CharacterList
		.UnwrapCensusCollection<Character>()
		.ToObservable().Test()
		.AssertResultCount(4)
		.AssertValues(Has.None.Null)
		.AssertValues(Has.One.Property(nameof(Character.Id)).EqualTo(5428016459719850385))
		.AssertValues(Has.Some.Property(nameof(Character.FactionId)).EqualTo(FactionId.NewConglomerate))
		.AssertValues(Has.One.Property(nameof(Character.Name)).Property("First").EqualTo("Naozumi"));

	[Test]
	public void DeserializeFactionList() => CensusJsonData.FactionList
		.UnwrapCensusCollection<Faction>()
		.ToObservable().Test()
		.AssertResultCount(5)
		.AssertValues(Has.None.Null)
		.AssertValues(Has.One.Property(nameof(Item.FactionId)).EqualTo(FactionId.None))
		.AssertValues(Has.One.Property(nameof(Item.FactionId)).EqualTo(FactionId.VanuSovereignty))
		.AssertValues(Has.One.Property(nameof(Item.FactionId)).EqualTo(FactionId.NewConglomerate))
		.AssertValues(Has.One.Property(nameof(Item.FactionId)).EqualTo(FactionId.TerranRepublic))
		.AssertValues(Has.One.Property(nameof(Item.FactionId)).EqualTo(FactionId.NSOperatives));

	[Test]
	public void DeserializeItemList() => CensusJsonData.ItemList
		.UnwrapCensusCollection<Item>()
		.ToObservable().Test()
		.AssertValueCount(10)
		.AssertValues(Has.None.Null)
		.AssertValues(Has.None.Property(nameof(Item.Weapon)).Not.Null)
		.AssertValues(Has.None.Property(nameof(Item.WeaponDatasheet)).Not.Null)
		.AssertValues(Has.One.Property(nameof(Item.Id)).EqualTo(2));

	[Test]
	public void DeserializeWeaponDatasheetList() => CensusJsonData.WeaponDatasheetList
		.UnwrapCensusCollection<WeaponDatasheet>()
		.ToObservable().Test()
		.AssertValueCount(100)
		.AssertValues(Has.None.Null)
		.AssertValues(Has.One.Property(nameof(WeaponDatasheet.Id)).EqualTo(2));

	[Test]
	public void DeserializeWeaponList() => CensusJsonData.WeaponList
		.UnwrapCensusCollection<Weapon>()
		.ToObservable().Test()
		.AssertValueCount(100)
		.AssertValues(Has.None.Null)
		.AssertValues(Has.One.Property(nameof(Weapon.Id)).EqualTo(2));
}