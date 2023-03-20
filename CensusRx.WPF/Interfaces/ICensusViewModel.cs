using CensusRx.Interfaces;

namespace CensusRx.WPF.Interfaces;

public interface ICensusViewModel<T>
	where T : ICensusObject
{
	T CensusObject { get; }
}
