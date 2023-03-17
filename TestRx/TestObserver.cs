using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace TestRx;

public class TestObserver<T> : IObserver<T>
{
	private int Completions { get; set; } = 0;
	private List<Exception> Exceptions { get; } = new();
	private List<T> Emissions { get; } = new();

	public string Label { get; set; } = string.Empty;
	public bool AutoLog { get; set; } = true;
	
	public TextWriter LogWriter { get; set; }

	public Action<TextWriter, Exception> ExceptionWriter { get; set; } = (writer, exception) =>
	{
		writer.WriteLine(exception.Message);
	};

	public Action<TextWriter, T> ValueWriter { get; set; } = (writer, value) =>
	{
		writer.WriteLine(value?.ToString() ?? "null");
	};
	
	public TestObserver()
	{
		this.LogWriter = TestContext.Out;
	}

	private void WriteLabel(TextWriter writer)
	{
		if (!string.IsNullOrEmpty(Label))
			writer.Write($"{Label}.");
	}

	public void OnCompleted()
	{
		Completions += 1;

		if (AutoLog)
		{
			this.WriteLabel(LogWriter);
			this.LogWriter.WriteLine("OnCompleted");
		}
	}

	public void OnError(Exception error)
	{
		Exceptions.Add(error);

		if (AutoLog)
		{
			this.WriteLabel(LogWriter);
			this.LogWriter.Write("OnError: ");
			this.ExceptionWriter.Invoke(this.LogWriter, error);
		}
	}

	public void OnNext(T value)
	{
		Emissions.Add(value);

		if (AutoLog)
		{
			this.WriteLabel(LogWriter);
			this.LogWriter.Write("OnNext: ");
			this.ValueWriter.Invoke(this.LogWriter, value);
		}
	}

	public TestObserver<T> LogExceptions(TextWriter writer)
	{
		if (!string.IsNullOrEmpty(Label))
		{
			writer.Write($"{Label}.");
		}

		writer.WriteLine($"exceptions: {Exceptions.Count}");

		var index = 0;

		foreach (var exception in Exceptions)
		{
			writer.Write($"{index++}: ");
			ExceptionWriter.Invoke(writer, exception);
		}

		return this;
	}

	public TestObserver<T> LogValues(TextWriter writer)
	{
		if (!string.IsNullOrEmpty(Label))
		{
			writer.Write($"{Label}.");
		}

		writer.WriteLine($"values: {Emissions.Count}");

		var index = 0;

		foreach (var value in Emissions)
		{
			writer.Write($"{index++}: ");
			ValueWriter.Invoke(writer, value);
		}

		return this;
	}

	public TestObserver<T> LogExceptions() => LogExceptions(this.LogWriter);
	public TestObserver<T> LogValues() => LogValues(this.LogWriter);

	public TestObserver<T> AssertCompletions(IResolveConstraint constraint, string message)
	{
		Assert.That(Completions, constraint, () => message);
		return this;
	}

	public TestObserver<T> AssertCompletedOnce() =>
		this.AssertCompletions(Is.LessThan(2), "Sequence completed more than once")
			.AssertCompletions(Is.GreaterThan(0), "Sequence did not complete");

	public TestObserver<T> AssertDidNotComplete() =>
		this.AssertCompletions(Is.EqualTo(0), "Sequence completed one or more times");

	public TestObserver<T> AssertExceptions(IResolveConstraint constraint, string? message = default)
	{
		Assert.That(Exceptions, constraint, message);
		return this;
	}

	public TestObserver<T> AssertExceptions(params Exception[] exceptions) =>
		this.AssertExceptions(Is.EquivalentTo(exceptions))
			.AssertDidNotComplete();

	public TestObserver<T> AssertSingleException()
	{
		AssertDidNotComplete();
		Assert.That(Exceptions, Has.Count.EqualTo(1));
		return this;
	}

	public TestObserver<T> AssertSingleException(IResolveConstraint constraint)
	{
		AssertSingleException();
		Assert.That(Exceptions.First(), constraint);
		return this;
	}

	public TestObserver<T> AssertSingleException<TException>() =>
		AssertSingleException(Is.AssignableTo<TException>());

	public TestObserver<T> AssertNoExceptions() =>
		this.AssertExceptions(Is.Empty, "Sequence threw one or more exceptions");

	public TestObserver<T> AssertValues(IResolveConstraint constraint)
	{
		Assert.That(Emissions, constraint);
		return this;
	}

	public TestObserver<T> AssertValueCount(int count) => AssertValues(Has.Count.EqualTo(count));

	public TestObserver<T> AssertValues(params T[] values) =>
		AssertValueCount(values.Length).AssertValues(Is.EquivalentTo(values));

	public TestObserver<T> AssertLastValue(IResolveConstraint constraint)
	{
		Assert.That(Emissions.Last(), constraint);
		return this;
	}

	public TestObserver<T> AssertLastValue(T value) => AssertLastValue(Is.EqualTo(value));

	public TestObserver<T> AssertCompletion() =>
		this.AssertNoExceptions().AssertCompletedOnce();

	public TestObserver<T> AssertResults(params T[] values) =>
		this.AssertCompletion().AssertValues(values);

	public TestObserver<T> AssertResultCount(int count) =>
		this.AssertCompletion().AssertValueCount(count);
}
