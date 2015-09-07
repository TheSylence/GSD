using GSD.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Windows;
using System.Windows.Documents;

namespace GSD.Tests.Converters
{
	[TestClass]
	public class TextToFlowDocumentConverterTests
	{
		[TestMethod, TestCategory( "Converters" )]
		public void ConvertCreatesFlowDocument()
		{
			// Arrange
			var conv = new TextToFlowDocumentConverter();

			// Act
			object converted = conv.Convert( string.Empty, null, null, null );

			// Assert
			Assert.IsInstanceOfType( converted, typeof( FlowDocument ) );
			Assert.IsNotNull( converted );
		}

		[TestMethod, TestCategory( "Converters" )]
		public void ConvertNullReturnsUnset()
		{
			// Arrange
			var conv = new TextToFlowDocumentConverter();

			// Act
			object converted = conv.Convert( null, null, null, null );

			// Assert
			Assert.AreEqual( DependencyProperty.UnsetValue, converted );
		}

		[TestMethod, TestCategory( "Converters" )]
		public void ConvertBackThrowsException()
		{
			// Arrange
			var conv = new TextToFlowDocumentConverter();

			// Act

			// Assert
			ExceptionAssert.Throws<NotSupportedException>( () => conv.ConvertBack( null, null, null, null ) );
		}

		[TestMethod, TestCategory( "Converters" )]
		public void MarkdownInstanceIsUsed()
		{
			// Arrange
			var mdMock = new Mock<Markdown>();
			mdMock.Setup( md => md.Transform( It.IsAny<string>() ) ).Returns( new FlowDocument() );

			var conv = new TextToFlowDocumentConverter
			{
				Markdown = mdMock.Object
			};

			// Act
			object converted = conv.Convert( string.Empty, null, null, null );

			// Assert
			Assert.IsInstanceOfType( converted, typeof( FlowDocument ) );
			Assert.IsNotNull( converted );

			mdMock.VerifyAll();
		}
	}
}