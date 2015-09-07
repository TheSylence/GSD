using GSD.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Windows.Media;

namespace GSD.Tests.Converters
{
	[TestClass]
	public class BackgroundColorTests
	{
		[TestMethod, TestCategory( "Converters" )]
		public void ConvertCreatesCorrectBrush()
		{
			// Arrange
			var conv = new BackgroundColor();

			// Act
			object invalid = conv.Convert( 123, null, null, null );
			object empty = conv.Convert( null, null, null, null );
			SolidColorBrush brush = (SolidColorBrush)conv.Convert( "A1B2C3", null, null, null );

			// Assert
			Assert.AreEqual( DependencyProperty.UnsetValue, invalid );
			Assert.AreEqual( DependencyProperty.UnsetValue, empty );

			Assert.AreEqual( 161, brush.Color.R );
			Assert.AreEqual( 178, brush.Color.G );
			Assert.AreEqual( 195, brush.Color.B );
		}

		[TestMethod, TestCategory( "Converters" )]
		public void ConvertBackThrowsException()
		{
			// Arrange
			var conv = new BackgroundColor();

			// Act

			// Assert
			ExceptionAssert.Throws<NotSupportedException>( () => conv.ConvertBack( null, null, null, null ) );
		}
	}
}