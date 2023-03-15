using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CensusRx.WPF;

public static class Utility
{
	private static Func<object> TypeFactory<T>(TypeInfo typeInfo)
	{
		var parameterlessConstructor = typeInfo.DeclaredConstructors.FirstOrDefault(ci => ci.IsPublic && ci.GetParameters().Length == 0);
		if (parameterlessConstructor is null)
		{
			throw new Exception($"Failed to register type {typeInfo.FullName} because it's missing a parameterless constructor.");
		}

		return Expression.Lambda<Func<object>>(Expression.New(parameterlessConstructor)).Compile();
	}

	private static Func<T> TypeFactory<T>()
	{
		var parameterlessConstructor = typeof(T).GetTypeInfo().DeclaredConstructors.FirstOrDefault(ci => ci.IsPublic && ci.GetParameters().Length == 0);
		if (parameterlessConstructor is null)
		{
			throw new Exception($"Failed to register type {typeof(T).FullName} because it's missing a parameterless constructor.");
		}

		return Expression.Lambda<Func<T>>(Expression.New(parameterlessConstructor)).Compile();
	}
}