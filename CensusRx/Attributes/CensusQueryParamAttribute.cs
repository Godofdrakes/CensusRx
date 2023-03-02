using System.Runtime.CompilerServices;

namespace CensusRx.Attributes;

public class CensusQueryParamAttribute : Attribute
{
	public string Name { get; }

	public CensusQueryParamAttribute([CallerMemberName] string name = "")
	{
		this.Name = name.ToLower();
	}
}