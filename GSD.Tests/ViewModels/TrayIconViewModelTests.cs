using GSD.ViewModels;
using GSD.ViewModels.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GSD.Tests.ViewModels
{
	[TestClass]
	public class TrayIconViewModelTests
	{
		[TestMethod, TestCategory( "ViewModels" )]
		public void QuitTriggersAppShutdown()
		{
			// Arrange
			var controllerMock = new Mock<IAppController>();
			controllerMock.Setup( x => x.Shutdown() ).Verifiable();

			var vm = new TrayIconViewModel( controllerMock.Object );

			// Act
			vm.QuitCommand.Execute( null );

			// Assert
			controllerMock.VerifyAll();
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ShowWindowTriggersWindowCommand()
		{
			// Arrange
			var controllerMock = new Mock<IAppController>();
			controllerMock.Setup( x => x.ShowWindow() ).Verifiable();

			var vm = new TrayIconViewModel( controllerMock.Object );

			// Act
			vm.ShowWindowCommand.Execute( null );

			// Assert
			controllerMock.VerifyAll();
		}
	}
}