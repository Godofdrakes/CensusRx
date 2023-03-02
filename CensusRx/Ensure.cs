using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CensusRx;

public static class Ensure
{
	public static void IsNotEmpty([NotNull] string? argument,
		[CallerArgumentExpression("argument")] string? paramName = null)
	{
		if (string.IsNullOrEmpty(argument))
		{
			ThrowArgumentNull(paramName);
		}
	}

	public static void IsPositive(int argument, [CallerArgumentExpression("argument")] string? paramName = null)
	{
		if (argument < 1)
		{
			ThrowArgumentOutOfRange(paramName);
		}
	}

	[DoesNotReturn]
	private static void ThrowArgumentNull(string? paramName) =>
		throw new ArgumentNullException(paramName);

	[DoesNotReturn]
	private static void ThrowArgumentOutOfRange(string? paramName) =>
		throw new ArgumentOutOfRangeException(paramName);
}