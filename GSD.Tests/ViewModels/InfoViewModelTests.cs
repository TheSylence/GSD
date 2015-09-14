using GSD.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GSD.Tests.ViewModels
{
	[TestClass]
	public class InfoViewModelTests
	{
		[TestMethod, TestCategory( "ViewModels" )]
		public void CloseCommandsTriggeresEvent()
		{
			// Arrange
			var vm = new InfoViewModel();
			bool received = false;

			vm.CloseRequested += ( s, e ) => received = true;

			// Act
			vm.CloseCommand.Execute( null );

			// Assert
			Assert.IsTrue( received );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void VersionIsPopulated()
		{
			// Arrange
			var vm = new InfoViewModel();

			// Act
			string version = vm.Version.ToString();

			// Assert
			Assert.IsNotNull( version );
			Assert.AreNotEqual( "0.0.0.0", version );
		}
	}
}