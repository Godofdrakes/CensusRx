using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using DynamicData;
using ReactiveUI;

namespace CensusRx.WPF;

[AttributeUsage(AttributeTargets.Property)]
public class PropertyReferenceAttribute : Attribute { }

public class PropertyReference : ReactiveObject
{
	public string Name => PropertyInfo.Name;
	public Type ValueType => PropertyInfo.PropertyType;
	public bool CanRead => PropertyInfo.CanRead;
	public bool CanWrite => PropertyInfo.CanWrite;

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

		SourceObject = sourceObject;
		PropertyInfo = propertyInfo;
		_value = sourceObject.Changed
			.Where(args => args.PropertyName == propertyInfo.Name)
			.Select(_ => GetValue())
			.ToProperty(this, model => model.Value, GetValue);
	}
}

public class PropertyReferenceRegistry
{
	public ReadOnlyObservableCollection<PropertyReference> PropertyReferences => _propertyReferences;

	private readonly ReadOnlyObservableCollection<PropertyReference> _propertyReferences;

	private readonly SourceCache<PropertyReference, PropertyInfo> _propertyCache;

	public PropertyReferenceRegistry()
	{
		_propertyCache = new SourceCache<PropertyReference, PropertyInfo>(reference => reference.PropertyInfo);
		_propertyCache.Connect().Bind(out _propertyReferences).Subscribe();
	}

	public void RegisterPropertySource(ReactiveObject sourceObject)
	{
		var properties = sourceObject.GetType().GetProperties()
			.Where(prop => prop.GetCustomAttribute<PropertyReferenceAttribute>() is not null)
			.Select(prop => new PropertyReference(sourceObject, prop));

		_propertyCache.AddOrUpdate(properties);
	}
}