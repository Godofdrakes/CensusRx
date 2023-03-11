using ReactiveUI;

namespace CensusRx.WPF.Views;

public abstract class CensusUserControl<T> : ReactiveUserControl<T>
	where T : class
{
	protected CensusUserControl()
	{
		this.WhenAnyValue(control => control.ViewModel)
			.BindTo(this, control => control.DataContext);
	}
}