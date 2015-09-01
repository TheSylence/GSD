using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GSD.Converters
{
	internal class ForegroundColor : IValueConverter
	{
		/// <summary>
		/// Converts a value.
		/// </summary>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		/// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			string str = value as string;
			if( str == null )
			{
				return DependencyProperty.UnsetValue;
			}

			var color = Color.FromRgb(
				byte.Parse( str.Substring( 0, 2 ), NumberStyles.HexNumber ),
				byte.Parse( str.Substring( 2, 2 ), NumberStyles.HexNumber ),
				byte.Parse( str.Substring( 4, 2 ), NumberStyles.HexNumber )
				);

			float cmax = Math.Max( Math.Max( color.ScB, color.ScG ), color.ScR );
			float cmin = Math.Min( Math.Max( color.ScB, color.ScG ), color.ScR );
			//float delta = cmax - cmin;

			float lightness = ( cmax + cmin ) / 2.0f;

			return new SolidColorBrush( lightness <= 0.5 ? Colors.White : Colors.Black );
		}

		/// <summary>
		/// Converts a value.
		/// </summary>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		/// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotSupportedException();
		}
	}
}