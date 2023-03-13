using System.Reflection;
using CensusRx.WPF.Interfaces;
using ReactiveUI;
using Splat;

namespace CensusRx.WPF.ServiceModules;

public class ReactiveViewForModule : IServiceModule
{
	public void Register(IMutableDependencyResolver locator)
	{
		Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
	}

	public void Startup(IReadonlyDependencyResolver locator) { }
}