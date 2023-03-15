using System.Reflection;

namespace CensusRx.Services;

public static class AssemblyEx
{
	public static IEnumerable<TypeInfo> GetServices(this Assembly assembly) =>
		assembly.DefinedTypes.Where(TypeEx.IsService);
}