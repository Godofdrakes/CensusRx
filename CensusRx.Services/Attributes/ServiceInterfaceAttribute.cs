using JetBrains.Annotations;

namespace CensusRx.Services;

[AttributeUsage(AttributeTargets.Interface), MeansImplicitUse]
public class ServiceInterfaceAttribute : Attribute { }