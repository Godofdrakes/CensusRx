using System.Reflection;

namespace CensusRx.Services;

public static class TypeEx
{
	public static bool IsServiceInterface(this Type type) =>
		type.IsInterface && type.GetCustomAttribute<ServiceInterfaceAttribute>() is not null;

	public static bool IsService(this TypeInfo type) =>
		!type.IsAbstract && type.ImplementedInterfaces.Any(IsServiceInterface);

	public static Type GetServiceInterface(this TypeInfo type) =>
		type.ImplementedInterfaces.Where(IsServiceInterface).First();
}