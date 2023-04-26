using System;
using System.Globalization;
using System.Windows.Data;

namespace CensusRx.WPF.Common;

public class OneWayValueConverter : IValueConverter
{
	public delegate object? ConvertFunction(object? value, Type targetType, object? parameter, CultureInfo culture);

	private readonly ConvertFunction _convertFunction;

	public OneWayValueConverter(ConvertFunction convertFunction)
	{
		_convertFunction = convertFunction ?? throw new ArgumentNullException(nameof(convertFunction));
	}

	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{ 
		return _convertFunction(value, targetType, parameter, culture);
	}

	public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}

	public static OneWayValueConverter Create(ConvertFunction convertFunction)
	{
		return new OneWayValueConverter(convertFunction);
	}

	public static OneWayValueConverter Create(Func<object?, object?> convertFunction)
	{
		return new OneWayValueConverter((value, _, _, _) => convertFunction(value));
	}
}