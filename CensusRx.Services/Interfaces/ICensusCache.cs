using System.Reactive.Linq;
using CensusRx.Interfaces;
using CensusRx.Services.Attributes;

namespace CensusRx.Services.Interfaces;

[ServiceInterface]
public interface ICensusCache
{
	IObservable<ICensusObject> Get(Type type, long id);
	IObservable<T> Get<T>(long id) where T : ICensusObject =>
		Get(typeof(T), id).OfType<T>();

	void Precache(IObservable<ICensusObject> observable);

	void Precache<T>(IObservable<T> observable) where T : ICensusObject =>
		Precache((IObservable<ICensusObject>)observable);
}