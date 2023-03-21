using System;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;
using CensusRx.Interfaces;
using CensusRx.WPF.Views;

namespace CensusRx.WPF.Markup;

public class ViewModelTemplate : MarkupExtension
{
	public Type Model { get; set; } = typeof(ICensusObject);

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		if (!Model.GetTypeInfo().ImplementedInterfaces.Contains(typeof(ICensusObject)))
		{
			throw new InvalidOperationException("Model must be of type ICensusObject");
		}

		return typeof(CensusUserControl<>).MakeGenericType(Model);
	}
}