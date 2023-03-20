namespace CensusRx.Test;

[TestFixture]
public class CensusMatchTests
{
	[Test]
	public void TestIsEqual() => Assert.That(CensusMatch.IsEqualTo("foo").ToString(), Is.EqualTo("foo"));

	[Test]
	public void TestStartsWith() => Assert.That(CensusMatch.StartsWith("foo").ToString(), Is.EqualTo("^foo"));

	[Test]
	public void TestContains() => Assert.That(CensusMatch.Contains("foo").ToString(), Is.EqualTo("*foo"));
}