using System.Diagnostics;
using System.Text.Json;
using CensusRx.Model;

namespace CensusRx;

public enum CensusOperand
{
	// (=)
	IsEqualTo,

	// (<) Values that are "less than" to the attributeValue
	LessThan,

	// ([) Values that are "less than or equal" to the attributeValue
	LessThanOrEqualTo,

	// (>) Values that are "greater than" the attributeValue
	GreaterThan,

	// (]) Values that are "greater than or equal" the attributeValue
	GreaterThanOrEqualTo,

	// (^) Values that start with a string in the attributeValue (like 'value%')
	StartsWith,

	// (*) Values that contain a string in the attributeValue (like '%value%')
	Contains,

	// (!) Values that do NOT have the given attributeValue
	IsNot,
}

[StackTraceHidden, DebuggerStepThrough]
public static class CensusMatch
{
	private static Dictionary<CensusOperand, char> OperandPrefix { get; } = new()
	{
		{ CensusOperand.LessThan, '<' },
		{ CensusOperand.LessThanOrEqualTo, '[' },
		{ CensusOperand.GreaterThan, '>' },
		{ CensusOperand.GreaterThanOrEqualTo, ']' },
		{ CensusOperand.StartsWith, '^' },
		{ CensusOperand.Contains, '*' },
		{ CensusOperand.IsNot, '!' },
	};
	
	private static string Prefix(CensusOperand operand, string value) => operand switch
	{
		CensusOperand.IsEqualTo => value,
		CensusOperand.LessThan => '<' + value,
		CensusOperand.LessThanOrEqualTo => '[' + value,
		CensusOperand.GreaterThan => '>' + value,
		CensusOperand.GreaterThanOrEqualTo => ']' + value,
		CensusOperand.StartsWith => '^' + value,
		CensusOperand.Contains => '*' + value,
		CensusOperand.IsNot => '!' + value,
		_ => throw new ArgumentOutOfRangeException(nameof(operand)),
	};

	public static string IsEqualTo(string value) => Prefix(CensusOperand.IsEqualTo, value);
	public static string IsEqualTo(object value, JsonSerializerOptions? serializerOptions) => IsEqualTo(JsonSerializer.Serialize(value, serializerOptions));

	public static string LessThan(string value) => Prefix(CensusOperand.LessThan, value);

	public static string LessThanOrEqualTo(string value) => Prefix(CensusOperand.LessThanOrEqualTo, value);

	public static string GreaterThan(string value) => Prefix(CensusOperand.GreaterThan, value);

	public static string GreaterThanOrEqualTo(string value) => Prefix(CensusOperand.GreaterThanOrEqualTo, value);

	public static string StartsWith(string value) => Prefix(CensusOperand.StartsWith, value);

	public static string Contains(string value) => Prefix(CensusOperand.Contains, value);

	public static string IsNot(string value) => Prefix(CensusOperand.IsNot, value);
}