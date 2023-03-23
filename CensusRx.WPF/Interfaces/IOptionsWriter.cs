using System;
using Microsoft.Extensions.Options;

namespace CensusRx.WPF.Interfaces;

public interface IOptionsWriter<out T> : IOptions<T>
	where T : class, new()
{
	T Get(string? name);

	void Write(Action<T> writeAction);
}