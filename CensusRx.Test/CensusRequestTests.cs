using CensusRx.Interfaces;

namespace CensusRx.Test;

[TestFixture]
public class CensusRequestTests
{
	private static CensusRequest<T> MakeRequest<T>(RequestBuilder<T> builder)
		where T : ICensusObject
	{
		var request = new CensusRequest<T>
		{
			NamingPolicy = CensusJson.NamingPolicy,
		};

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

		Assert.Multiple(() =>
		{
			Assert.That(request.QueryParams, Contains.Key("name.first_lower").WithValue("^naozumi"));
			Assert.That(request.QueryParams, Contains.Key("c:limit").WithValue("10"));
		});
	}

	[Test]
	public void TestWeaponNameSearch()
	{
		var request = MakeRequest<Item>(request => request
			.Where(item => item.Name.En).Contains("reaper")
			.Join(item => item.WeaponDatasheet)
			.Join(item => item.Weapon)
			.CaseSensitive(false)
			.LimitTo(10));

		Assert.Multiple(() =>
		{
			Assert.That(request.QueryParams, Contains.Key("c:limit").WithValue("10"));
			Assert.That(request.QueryParams, Contains.Key("c:case").WithValue("false"));
			Assert.That(request.QueryParams, Contains.Key("name.en").WithValue("*reaper"));
			Assert.That(request.JoinArgs, Has.One.Property(nameof(CensusJoinArgs.Type)).EqualTo("weapon"));
			Assert.That(request.JoinArgs, Has.One.Property(nameof(CensusJoinArgs.InjectAt)).EqualTo("weapon"));
			Assert.That(request.JoinArgs, Has.One.Property(nameof(CensusJoinArgs.Type)).EqualTo("weapon_datasheet"));
			Assert.That(request.JoinArgs, Has.One.Property(nameof(CensusJoinArgs.InjectAt)).EqualTo("weapon_datasheet"));
		});
	}
}