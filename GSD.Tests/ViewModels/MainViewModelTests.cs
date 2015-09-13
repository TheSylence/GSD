using System;
using System.Threading.Tasks;
using GSD.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GSD.Tests.ViewModels
{
	[TestClass]
	public class MainViewModelTests
	{
		[TestMethod, TestCategory( "ViewModels" )]
		public void DatabaseConnectionIsEstablishedInConstructor()
		{
			// Arrange
			var connectorMock = new Mock<IDatabaseConnector>();
			connectorMock.Setup( x => x.ConnectToDatabase() ).Verifiable();

			var runnerMock = new Mock<ITaskRunner>();
			runnerMock.Setup( x => x.Run( It.IsAny<Action>() ) ).Returns<Action>( a =>
			{
				try
				{
					a();
				}
				catch
				{
				}

				return Task.FromResult<object>( null );
			} );

			// Act
			var vm = new MainViewModel( connectorMock.Object, runnerMock.Object );

			// Assert
			connectorMock.VerifyAll();
		}
	}
}