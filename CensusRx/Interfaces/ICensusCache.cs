namespace CensusRx.Interfaces;

public interface ICensusCache<out T>
	where T : ICensusViewModel
{
	public IObservable<T> Get(long id);
}
