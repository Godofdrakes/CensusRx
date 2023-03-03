using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace TestRx.Test;

[TestFixture]
public class TestObserverTests
{
	public static TestObserver<int> TestValues([CallerMemberName] string label = "")
	{
		var observer = new TestObserver<int> { Label = label };
		Observable.Range(1, 3).Subscribe(observer);
		return observer;
	}

	[Test]
	public void AssertValues() => TestValues().AssertValues(1, 2, 3);

	[Test]
	public void AssertNoExceptions() => TestValues().AssertNoExceptions();

	[Test]
	public void AssertCompletedOnce() => TestValues().AssertCompletedOnce();

	[Test]
	public void AssertResults() => TestValues().AssertResults(1, 2, 3);

	[Test]
	public void AssertResultCount() => TestValues().AssertResultCount(3);

	[Test]
	public void AssertValueCount() => TestValues().AssertValueCount(3);
}