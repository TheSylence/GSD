using GSD.ViewServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GSD.Tests.ViewServices
{
	[TestClass]
	public class ConfirmationServiceArgsTests
	{
		[TestMethod, TestCategory( "ViewServices" )]
		public void ConstructorSetsCorrectProperties()
		{
			// Arrange

			// Act
			var csa = new ConfirmationServiceArgs( "title", "message", "ok", "cancel" );

			// Assert
			Assert.AreEqual( "title", csa.Title );
			Assert.AreEqual( "message", csa.Message );
			Assert.AreEqual( "ok", csa.OkText );
			Assert.AreEqual( "cancel", csa.CancelText );
		}
	}
}