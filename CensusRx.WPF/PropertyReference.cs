using System;
using System.Reactive.Linq;
using System.Reflection;
using JetBrains.Annotations;
using ReactiveUI;

namespace CensusRx.WPF;

[AttributeUsage(AttributeTargets.Property), MeansImplicitUse]
public class PropertyReferenceAttribute : Attribute
{
	public PropertyReferenceAttribute(bool isReadOnly = false)
	{
		IsReadOnly = isReadOnly;
	}

	public bool IsReadOnly { get; }
}

public class PropertyReference : ReactiveObject
{
	public string Name => PropertyInfo.Name;
	public Type ValueType => PropertyInfo.PropertyType;
	public bool IsReadOnly { get; }

	public object? Value
	{
		get => _value.Value;
		set => SetValue(value);
	}

	public ReactiveObject SourceObject { get; }
	public PropertyInfo PropertyInfo { get; }

	private readonly ObservableAsPropertyHelper<object?> _value;

	private object? GetValue() => PropertyInfo.GetValue(SourceObject);
	private void SetValue(object? value) => PropertyInfo.SetValue(SourceObject, value);

	public PropertyReference(ReactiveObject sourceObject, PropertyInfo propertyInfo)
	{
		if (!propertyInfo.CanRead)
			throw new ArgumentException($"{propertyInfo.Name} is not a readable property", nameof(propertyInfo));

		var attribute = propertyInfo.GetCustomAttribute<PropertyReferenceAttribute>();

		SourceObject = sourceObject;
		PropertyInfo = propertyInfo;
		_value = sourceObject.Changed
			.Where(args => args.PropertyName == propertyInfo.Name)
			.Select(_ => GetValue())
			.ToProperty(this, model => model.Value, GetValue);

		IsReadOnly = !propertyInfo.CanWrite || attribute?.IsReadOnly == true;
	}
}