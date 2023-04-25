using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace TestRx.Test;

[TestFixture]
public static class TestObserverTests
{
	private static TestObserver<T> ObserveForTest<T>(this IObservable<T> sequence, [CallerMemberName] string label = "")
	{
		var observer = new TestObserver<T> { Label = label };
		sequence.Subscribe(observer);
		return observer;
	}

	[Test]
	public static void AssertValueCount()
	{
		Assert.That(() => Observable.Range(1, 3)
				.ObserveForTest()
				.AssertValueCount(3),
			Throws.Nothing);

		Assert.That(() => Observable.Range(1, 4)
				.ObserveForTest()
				.AssertValueCount(3),
			Throws.TypeOf<AssertionException>());
	}

	[Test]
	public static void AssertValues()
	{
		Assert.That(() => Observable.Range(1, 3)
				.ObserveForTest()
				.AssertValues(1, 2, 3),
			Throws.Nothing);

		Assert.That(() => Observable.Range(1, 3)
				.ObserveForTest()
				.AssertValues(1, 2, 3, 4),
			Throws.TypeOf<AssertionException>());
	}

	[Test]
	public static void AssertNoExceptions()
	{
		Assert.That(() => Observable.Range(1, 3)
				.ObserveForTest()
				.AssertNoExceptions(),
			Throws.Nothing);

		Assert.That(() => Observable.Throw<int>(new Exception())
				.ObserveForTest()
				.AssertNoExceptions(),
			Throws.TypeOf<AssertionException>());
	}

	[Test]
	public static void AssertCompletedOnce()
	{
		Assert.That(() => Observable.Range(1, 3)
				.ObserveForTest()
				.AssertCompletedOnce(),
			Throws.Nothing);

		Assert.That(() => Observable.Never<int>()
				.ObserveForTest()
				.AssertCompletedOnce(),
			Throws.TypeOf<AssertionException>());
	}
}