using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using CensusRx.WPF.Interfaces;
using DynamicData;
using ReactiveUI;
using Splat;

namespace CensusRx.WPF.ServiceModules;

public class PropertyReferenceRegistry : IServiceModule
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
			.Where(prop => CustomAttributeExtensions.GetCustomAttribute<PropertyReferenceAttribute>((MemberInfo)prop) is not null)
			.Select(prop => new PropertyReference(sourceObject, prop));

		_propertyCache.AddOrUpdate(properties);
	}

	public void Register(IMutableDependencyResolver locator)
	{
		locator.RegisterConstant(this);
	}

	public void Startup(IReadonlyDependencyResolver locator) { }
}