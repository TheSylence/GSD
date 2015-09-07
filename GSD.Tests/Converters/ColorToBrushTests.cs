using GSD.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Windows.Media;

namespace GSD.Tests.Converters
{
	[TestClass]
	public class ColorToBrushTests
	{
		[TestMethod, TestCategory( "Converters" )]
		public void ConvertBackThrowsException()
		{
			// Arrange
			var conv = new ColorToBrush();

			// Act

			// Assert
			ExceptionAssert.Throws<NotSupportedException>( () => conv.ConvertBack( null, null, null, null ) );
		}

		[TestMethod, TestCategory( "Converters" )]
		public void ConvertCreatesCorrectBrush()
		{
			// Arrange
			var conv = new ColorToBrush();

			// Act
			object converted = conv.Convert( Colors.Yellow, null, null, null );

			// Assert
			SolidColorBrush brush = converted as SolidColorBrush;
			Assert.IsNotNull( brush );
			Assert.AreEqual( Colors.Yellow, brush.Color );
		}

		[TestMethod, TestCategory( "Converters" )]
		public void ConvertInvalidTypeReturnsUnset()
		{
			// Arrange
			var conv = new ColorToBrush();

			// Act
			object converted = conv.Convert( string.Empty, null, null, null );

			// Assert
			Assert.AreEqual( DependencyProperty.UnsetValue, converted );
		}
	}
}