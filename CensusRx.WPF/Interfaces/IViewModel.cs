using ReactiveUI;

namespace CensusRx.WPF.Interfaces;

public interface IViewModel :
	IReactiveNotifyPropertyChanged<IReactiveObject>,
	IHandleObservableErrors,
	IReactiveObject
{
	// Basically just a tag interface
}