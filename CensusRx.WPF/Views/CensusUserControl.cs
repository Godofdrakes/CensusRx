using CensusRx.Services;
using CensusRx.Services.Attributes;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace CensusRx.WPF.Views;

[ServiceLifetime(ServiceLifetime.Transient)]
public abstract class CensusUserControl<T> : ReactiveUserControl<T>
	where T : class
{
	protected CensusUserControl()
	{
		this.WhenActivated(d =>
		{
			this.WhenAnyValue(control => control.ViewModel)
				.BindTo(this, control => control.DataContext);
		});
	}
}