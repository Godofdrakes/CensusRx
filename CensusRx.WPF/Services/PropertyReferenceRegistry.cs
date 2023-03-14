using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using DynamicData;
using ReactiveUI;

namespace CensusRx.WPF.Services;

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