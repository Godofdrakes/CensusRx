using System.Diagnostics;

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
public readonly record struct CensusMatch(CensusOperand Operand, string Value)
{
	public static CensusMatch IsEqualTo(string value) => new(CensusOperand.IsEqualTo, value);
	public static CensusMatch IsEqualTo(int value) => new(CensusOperand.IsEqualTo, value.ToString());

	public static CensusMatch LessThan(string value) => new(CensusOperand.LessThan, value);
	public static CensusMatch LessThan(int value) => new(CensusOperand.LessThan, value.ToString());

	public static CensusMatch LessThanOrEqualTo(string value) => new(CensusOperand.LessThanOrEqualTo, value);
	public static CensusMatch LessThanOrEqualTo(int value) => new(CensusOperand.LessThanOrEqualTo, value.ToString());

	public static CensusMatch GreaterThan(string value) => new(CensusOperand.GreaterThan, value);
	public static CensusMatch GreaterThan(int value) => new(CensusOperand.GreaterThan, value.ToString());

	public static CensusMatch GreaterThanOrEqualTo(string value) => new(CensusOperand.GreaterThanOrEqualTo, value);

	public static CensusMatch GreaterThanOrEqualTo(int value) =>
		new(CensusOperand.GreaterThanOrEqualTo, value.ToString());

	public static CensusMatch StartsWith(string value) => new(CensusOperand.StartsWith, value);

	public static CensusMatch Contains(string value) => new(CensusOperand.Contains, value);

	public static CensusMatch IsNot(string value) => new(CensusOperand.IsNot, value);
	public static CensusMatch IsNot(int value) => new(CensusOperand.IsNot, value.ToString());

	public override string ToString() => Operand switch
	{
		CensusOperand.IsEqualTo => Value,
		CensusOperand.LessThan => '<' + Value,
		CensusOperand.LessThanOrEqualTo => '[' + Value,
		CensusOperand.GreaterThan => '>' + Value,
		CensusOperand.GreaterThanOrEqualTo => ']' + Value,
		CensusOperand.StartsWith => '^' + Value,
		CensusOperand.Contains => '*' + Value,
		CensusOperand.IsNot => '!' + Value,
		_ => throw new ArgumentOutOfRangeException(),
	};
}