using GSD.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Windows.Media;

namespace GSD.Tests.Converters
{
	[TestClass]
	public class ForegroundColorTests
	{
		[TestMethod, TestCategory( "Converters" )]
		public void BlackAndWhiteBackgroundsResultsInWhiteAndBlackForeground()
		{
			// Arrange
			var conv = new ForegroundColor();

			// Act
			object white = conv.Convert( "000000", null, null, null );
			object black = conv.Convert( "ffffff", null, null, null );

			// Assert
			SolidColorBrush brush = white as SolidColorBrush;
			Assert.IsNotNull( brush );
			Assert.AreEqual( Colors.White, brush.Color );

			brush = black as SolidColorBrush;
			Assert.IsNotNull( brush );
			Assert.AreEqual( Colors.Black, brush.Color );
		}

		[TestMethod, TestCategory( "Converters" )]
		public void ConvertBackThrowsException()
		{
			// Arrange
			var conv = new ForegroundColor();

			// Act

			// Assert
			ExceptionAssert.Throws<NotSupportedException>( () => conv.ConvertBack( null, null, null, null ) );
		}

		[TestMethod, TestCategory( "Converters" )]
		public void ConvertInvalidValueResultsInUnset()
		{
			// Arrange
			var conv = new ForegroundColor();

			// Act
			object converted = conv.Convert( 123, null, null, null );

			// Assert
			Assert.AreEqual( DependencyProperty.UnsetValue, converted );
		}

		[TestMethod, TestCategory( "Converters" )]
		public void DarkGrayResultsInWhite()
		{
			// Arrange
			var conv = new ForegroundColor();

			// Act
			object color = conv.Convert( "444444", null, null, null );

			// Assert
			SolidColorBrush brush = color as SolidColorBrush;
			Assert.IsNotNull( brush );
			Assert.AreEqual( Colors.White, brush.Color );
		}

		[TestMethod, TestCategory( "Converters" )]
		public void LightGrayResultsInBlack()
		{
			// Arrange
			var conv = new ForegroundColor();

			// Act
			object color = conv.Convert( "cccccc", null, null, null );

			// Assert
			SolidColorBrush brush = color as SolidColorBrush;
			Assert.IsNotNull( brush );
			Assert.AreEqual( Colors.Black, brush.Color );
		}

		[TestMethod, TestCategory( "Converters" )]
		public void RedBackgroundResultsInWhiteColor()
		{
			// Arrange
			var conv = new ForegroundColor();

			// Act
			object color = conv.Convert( "ff0000", null, null, null );

			// Assert
			SolidColorBrush brush = color as SolidColorBrush;
			Assert.IsNotNull( brush );
			Assert.AreEqual( Colors.White, brush.Color );
		}
	}
}