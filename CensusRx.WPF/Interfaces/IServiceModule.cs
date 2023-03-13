using JetBrains.Annotations;
using Splat;

namespace CensusRx.WPF.Interfaces;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.Default)]
public interface IServiceModule
{
	void Register(IMutableDependencyResolver locator);
	void Startup(IReadonlyDependencyResolver locator);
}