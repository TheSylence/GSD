﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GSD.Converters
{
	internal class BackgroundColor : IValueConverter
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

			return new SolidColorBrush( color );
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