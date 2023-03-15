using JetBrains.Annotations;

namespace Microsoft.Extensions.DependencyInjection;

[AttributeUsage(AttributeTargets.Interface), MeansImplicitUse]
public class ServiceInterfaceAttribute : Attribute { }