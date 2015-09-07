using GSD.ViewServices;
using MahApps.Metro.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace GSD.Tests.ViewServices
{
	[TestClass]
	public class ViewServiceRepositoryTests
	{
		[TestMethod, TestCategory( "ViewServices" )]
		public void NotRegisteredServiceThrows()
		{
			// Arrange
			IViewServiceRepository repo = new ViewServiceRepository();

			// Act

			// Assert
			ExceptionAssert.Throws<InvalidOperationException, object>( () => repo.Execute<IDummyService>() );
		}

		[TestMethod, TestCategory( "ViewServices" )]
		public void RegisteredServiceIsExecuted()
		{
			// Arrange
			var serviceMock = new Mock<IViewService>();
			serviceMock.Setup( s => s.Execute( It.IsAny<MetroWindow>(), It.IsAny<object>() ) ).Returns( Task.FromResult( (object)123 ) );

			IViewServiceRepository repo = new ViewServiceRepository();
			repo.Register<IDummyService>( serviceMock.Object );

			// Act
			var result = repo.Execute<IDummyService>().Result;

			// Assert
			Assert.AreEqual( 123, result );
			serviceMock.VerifyAll();
		}

		[TestMethod, TestCategory( "ViewServices" )]
		public void ServiceResultIsCasted()
		{
			// Arrange
			var serviceMock = new Mock<IViewService>();
			serviceMock.Setup( s => s.Execute( It.IsAny<MetroWindow>(), It.IsAny<object>() ) ).Returns( Task.FromResult( (object)123 ) );

			IViewServiceRepository repo = new ViewServiceRepository();
			repo.Register<IDummyService>( serviceMock.Object );

			// Act
			int result = repo.Execute<IDummyService, int>().Result;

			// Assert
			Assert.AreEqual( 123, result );
			serviceMock.VerifyAll();
		}

		private interface IDummyService : IViewService
		{
		}
	}
}