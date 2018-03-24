using System;
using Windows.UI.Xaml.Data;

namespace Dht.Sample.Common
{
	public sealed class FloatToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return $"{value:0.0}{parameter}";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return System.Convert.ToSingle(value);
		}
	}
}