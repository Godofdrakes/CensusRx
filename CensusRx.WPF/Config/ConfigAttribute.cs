using System;
using JetBrains.Annotations;

namespace CensusRx.WPF.Config;

[MeansImplicitUse]
public class ConfigAttribute : Attribute
{
	public ConfigAttribute(string category)
	{
		Category = category;
	}

	public string Category { get; }
}