using GSD.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;

namespace GSD.Tests.Converters
{
	[TestClass]
	public class RelativeWidthTests
	{
		[TestMethod, TestCategory( "Converters" )]
		public void ConvertBackThrowsException()
		{
			// Arrange
			var conv = new RelativeWidth();

			// Act

			// Assert
			ExceptionAssert.Throws<NotSupportedException>( () => conv.ConvertBack( null, null, null, null ) );
		}

		[TestMethod, TestCategory( "Converters" )]
		public void ConvertMultipliesCorrectValues()
		{
			// Arrange
			var conv = new RelativeWidth();

			// Act
			object converted = conv.Convert( 100.0, null, "80", null );

			// Assert
			Assert.AreEqual( 80.0, converted );
		}

		[TestMethod, TestCategory( "Converters" )]
		public void ConvertWithInvalidParameterReturnsUnset()
		{
			// Arrange
			var conv = new RelativeWidth();

			// Act
			object empty = conv.Convert( 1.3, null, null, null );
			object invalid = conv.Convert( 1.3, null, string.Empty, null );

			// Assert
			Assert.AreEqual( DependencyProperty.UnsetValue, empty );
			Assert.AreEqual( DependencyProperty.UnsetValue, invalid );
		}
	}
}