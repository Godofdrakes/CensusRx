using System;
using System.Globalization;
using System.Windows.Data;

namespace CensusRx.WPF.Converters;

[ValueConversion(typeof(int), typeof(double))]
public class NumberConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is int i)
		{
			return (double) i;
		}

		throw new InvalidOperationException();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is double d)
		{
			return (int) d;
		}

		throw new InvalidOperationException();
	}
}