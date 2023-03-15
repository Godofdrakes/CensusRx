using System;
using CensusRx.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CensusRx.WPF.Interfaces;

[ServiceInterface]
public interface ICensusClient
{
	IObservable<string> Get<T>(RequestBuilder<T> requestBuilder) where T : ICensusObject;
	IObservable<int> Count<T>(RequestBuilder<T> requestBuilder) where T : ICensusObject;
}
