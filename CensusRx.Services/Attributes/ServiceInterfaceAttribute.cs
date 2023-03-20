using JetBrains.Annotations;

namespace CensusRx.Services.Attributes;

[AttributeUsage(AttributeTargets.Interface), MeansImplicitUse]
public class ServiceInterfaceAttribute : Attribute { }