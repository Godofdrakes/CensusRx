using System.Collections.Generic;

namespace CensusRx.WPF.Services;

public abstract class ConfigObject
{
	private readonly Dictionary<string, object?> _resetValues = new();
	
	public void Save()
	{
		foreach (var property in GetType().GetProperties())
		{
			_resetValues[property.Name] = property.GetValue(this);
		}
	}

	public void Reset()
	{
		foreach (var property in GetType().GetProperties())
		{
			property.SetValue(this, _resetValues[property.Name]);
		}
	}
}

public class ConfigService
{
	
}