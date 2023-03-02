using System.Reactive.Linq;

namespace TestRx.Test;

[TestFixture]
public class TestObserverTests
{
	[Test]
	public void AssertValues()
	{
		var observer = new TestObserver<int>()
		{
			Label = "observer",
			AutoLog = true,
		};

		Observable.Range(1, 3).Subscribe(observer);
		observer.LogValues();
		observer.AssertValues(1, 2, 3);
	}

	[Test]
	public void AssertNoExceptions()
	{
		var observer = new TestObserver<int>()
		{
			Label = "observer",
			AutoLog = true,
		};

		Observable.Range(1, 3).Subscribe(observer);
		observer.LogExceptions();
		observer.AssertNoExceptions();
	}

	[Test]
	public void AssertCompletedOnce()
	{
		var observer = new TestObserver<int>()
		{
			Label = "observer",
			AutoLog = true,
		};

		Observable.Range(1, 3).Subscribe(observer);
		observer.LogExceptions();
		observer.LogValues();
		observer.AssertCompletedOnce();
	}
}