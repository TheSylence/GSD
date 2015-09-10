using System.Windows;
using System.Windows.Controls;

namespace GSD.Views
{
	internal class Localization
	{
		public static object GetFormatSegment1( DependencyObject obj )
		{
			return obj.GetValue( FormatSegment1Property );
		}

		public static string GetStringFormat( DependencyObject obj )
		{
			return (string)obj.GetValue( StringFormatProperty );
		}

		public static void SetFormatSegment1( DependencyObject obj, object value )
		{
			obj.SetValue( FormatSegment1Property, value );
		}

		public static void SetStringFormat( DependencyObject obj, string value )
		{
			obj.SetValue( StringFormatProperty, value );
		}

		private static void OnValueChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			TextBlock txt = d as TextBlock;
			if( txt == null )
			{
				return;
			}

			txt.Text = string.Format( GetStringFormat( d ), GetFormatSegment1( d ) );
		}

		public static readonly DependencyProperty FormatSegment1Property = DependencyProperty.RegisterAttached( "FormatSegment1", typeof( object ), typeof( Localization ), new PropertyMetadata( null, OnValueChanged ) );
		public static readonly DependencyProperty StringFormatProperty = DependencyProperty.RegisterAttached( "StringFormat", typeof( string ), typeof( Localization ), new PropertyMetadata( null, OnValueChanged ) );
	}
}