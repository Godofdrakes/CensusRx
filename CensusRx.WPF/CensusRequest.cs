using System;
using System.Collections.Generic;
using CensusRx.Interfaces;
using CensusRx.WPF.Interfaces;

namespace CensusRx.WPF;

public sealed class CensusRequest<T> : ICensusRequest<T>
	where T : ICensusObject
{
	private string? TempString { get; set; }

	private readonly List<(string key, CensusMatch value)> _queryParams = new();

	public IReadOnlyList<(string key, CensusMatch value)> QueryParams => _queryParams;

	public ICensusRequest<T> Where(string query)
	{
		if (TempString is not null)
			throw new InvalidOperationException("TempString is not null");

		TempString = query;
		return this;
	}

	public ICensusRequest<T> Matches(CensusMatch censusMatch)
	{
		if (TempString is null)
			throw new InvalidOperationException("TempString is null");

		_queryParams.Add((TempString, censusMatch));
		TempString = null;
		return this;
	}

	public ICensusRequest<T> Bind(Action<(string key, CensusMatch value)> bindAction)
	{
		if (TempString is not null)
			throw new InvalidOperationException("TempString is not null");

		foreach (var pair in this._queryParams)
			bindAction.Invoke(pair);

		return this;
	}
}
