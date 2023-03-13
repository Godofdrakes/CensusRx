using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Splat;

namespace CensusRx.WPF;

public static class LocatorEx
{
	public static bool TryGetService<T>(
		this IReadonlyDependencyResolver locator,
		[MaybeNullWhen(false)] out T service,
		string? contract = default,
		[CallerMemberName] string caller = "",
		[CallerFilePath] string file = "",
		[CallerLineNumber] int line = 0)
	{
		service = locator.GetService<T>(contract);
		if (service is null)
		{
			Debug.WriteLine($"Failed to get '{typeof(T)}' for '{caller}' ({file}:{line}");
		}

		return service is not null;
	}
}