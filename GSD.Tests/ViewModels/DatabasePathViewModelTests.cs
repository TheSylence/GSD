using GSD.ViewModels;
using GSD.ViewServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace GSD.Tests.ViewModels
{
	[TestClass]
	public class DatabasePathViewModelTests
	{
		[TestMethod, TestCategory( "ViewModels" )]
		public void BrowseFolderCommandExecutesServices()
		{
			// Arrange
			var viewServiceMock = new Mock<IViewServiceRepository>();
			viewServiceMock.Setup( x => x.Execute<IBrowseFileService, string>( It.IsAny<object>() ) ).Returns( () => Task.FromResult( "123" ) );

			var vm = new DatabasePathViewModel( viewServiceMock.Object );

			// Act
			vm.BrowseFolderCommand.Execute( null );

			// Assert
			viewServiceMock.VerifyAll();
			Assert.AreEqual( "123", vm.Path );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void CancelCommandClosesDialog()
		{
			// Arrange
			var vm = new DatabasePathViewModel();
			bool received = false;

			vm.CloseRequested += ( s, e ) => received = true;

			// Act
			vm.CancelCommand.Execute( null );

			// Assert
			Assert.IsTrue( received );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void OkCommandClosesDialog()
		{
			// Arrange
			var vm = new DatabasePathViewModel();
			bool received = false;

			vm.CloseRequested += ( s, e ) => received = true;

			// Act
			vm.Path = "test";
			vm.OkCommand.Execute( null );

			// Assert
			Assert.IsTrue( received );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void OkCommandsNeedsPath()
		{
			// Arrange
			var vm = new DatabasePathViewModel();

			// Act
			vm.Path = string.Empty;
			bool empty = vm.OkCommand.CanExecute( null );

			vm.Path = "test";
			bool nonEmpty = vm.OkCommand.CanExecute( null );

			// Assert
			Assert.IsFalse( empty );
			Assert.IsTrue( nonEmpty );
		}
	}
}