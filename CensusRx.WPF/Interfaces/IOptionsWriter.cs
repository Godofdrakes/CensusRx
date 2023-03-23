using System;

namespace CensusRx.WPF.Interfaces;

public class OptionsWriterArguments<T>
{
	public T? Value { get; set; }
	public string? File { get; set; }
	public string? Section { get; set; }
}

public interface IOptionsWriter
{
	void Write<T>(Action<OptionsWriterArguments<T>> writeAction) where T : new();
}