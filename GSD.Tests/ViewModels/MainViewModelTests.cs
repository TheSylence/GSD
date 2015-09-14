using GalaSoft.MvvmLight.Messaging;
using GSD.Messages;
using GSD.ViewModels;
using GSD.ViewModels.Utilities;
using GSD.ViewServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace GSD.Tests.ViewModels
{
	[TestClass]
	public class MainViewModelTests
	{
		[ClassInitialize]
		public static void _Init( TestContext context )
		{
			TaskRunnerMock = new Mock<ITaskRunner>();
			TaskRunnerMock.Setup( x => x.Run( It.IsAny<Action>() ) ).Returns<Action>( a =>
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

			ConnectorMock = new Mock<IDatabaseConnector>();
			ViewServiceRepoMock = new Mock<IViewServiceRepository>();
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void AddEntryCannotBeExecutedBeforeLoading()
		{
			// Arrange
			var vm = new MainViewModel( ConnectorMock.Object, null, TaskRunnerMock.Object );

			// Act
			bool canExecute = vm.AddEntryCommand.CanExecute( null );

			// Assert
			Assert.IsFalse( canExecute );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void AddEntryNeedsCurrentProject()
		{
			// Arrange
			var projectListMock = new Mock<IProjectListViewModel>();
			ProjectViewModel currentProject = null;
			projectListMock.SetupSet( x => x.CurrentProject = It.IsAny<ProjectViewModel>() ).Callback<ProjectViewModel>( v => currentProject = v );
			projectListMock.SetupGet( x => x.CurrentProject ).Returns( () => currentProject );

			var vm = new MainViewModel( ConnectorMock.Object, null, TaskRunnerMock.Object, null, null, projectListMock.Object );

			// Act
			vm.ProjectList.CurrentProject = null;
			bool noSelection = vm.AddEntryCommand.CanExecute( null );

			vm.ProjectList.CurrentProject = new ProjectViewModel( new GSD.Models.Project() );
			bool selection = vm.AddEntryCommand.CanExecute( null );

			// Assert
			Assert.IsFalse( noSelection );
			Assert.IsTrue( selection );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void DatabaseConnectionIsEstablishedInConstructor()
		{
			// Arrange
			var connectorMock = new Mock<IDatabaseConnector>();
			connectorMock.Setup( x => x.ConnectToDatabase() ).Verifiable();

			// Act
			var vm = new MainViewModel( connectorMock.Object, null, TaskRunnerMock.Object );

			// Assert
			connectorMock.VerifyAll();
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void EditEntryCommandsNeedsParameter()
		{
			// Arrange
			var vm = new MainViewModel( ConnectorMock.Object, null, TaskRunnerMock.Object );

			// Act
			bool noArg = vm.EditEntryCommand.CanExecute( null );
			bool invalidArg = vm.EditEntryCommand.CanExecute( string.Empty );
			bool withArg = vm.EditEntryCommand.CanExecute( new TodoViewModel( new GSD.Models.Todo() ) );

			// Assert
			Assert.IsFalse( noArg );
			Assert.IsFalse( invalidArg );
			Assert.IsTrue( withArg );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void InfoCommandOpensDialog()
		{
			// Arrange
			var viewServiceRepoMock = new Mock<IViewServiceRepository>();
			viewServiceRepoMock.Setup( x => x.Execute<IInfoService>( It.IsAny<object>() ) ).Returns( () => Task.FromResult<object>( null ) ).Verifiable();

			var vm = new MainViewModel( ConnectorMock.Object, viewServiceRepoMock.Object, TaskRunnerMock.Object );

			// Act
			vm.InfoCommand.Execute( null );

			// Assert
			viewServiceRepoMock.VerifyAll();
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void OpenTagsCommandNeedsProject()
		{
			// Arrange
			var projectListMock = new Mock<IProjectListViewModel>();
			ProjectViewModel currentProject = null;
			projectListMock.SetupSet( x => x.CurrentProject = It.IsAny<ProjectViewModel>() ).Callback<ProjectViewModel>( v => currentProject = v );
			projectListMock.SetupGet( x => x.CurrentProject ).Returns( () => currentProject );

			var vm = new MainViewModel( ConnectorMock.Object, null, TaskRunnerMock.Object, null, null, projectListMock.Object );

			// Act
			vm.ProjectList.CurrentProject = null;
			bool noSelection = vm.OpenTagManagementCommand.CanExecute( null );

			vm.ProjectList.CurrentProject = new ProjectViewModel( new GSD.Models.Project() );
			bool selection = vm.OpenTagManagementCommand.CanExecute( null );

			// Assert
			Assert.IsFalse( noSelection );
			Assert.IsTrue( selection );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void OpenTagsCommandSendsMessage()
		{
			// Arrange
			var projectListMock = new Mock<IProjectListViewModel>();
			projectListMock.SetupGet( x => x.CurrentProject ).Returns( new ProjectViewModel( new GSD.Models.Project() ) );

			var messengerMock = new Mock<IMessenger>();
			messengerMock.Setup( x => x.Send( It.Is<FlyoutMessage>( msg => msg.FlyoutName == "TagsFlyout" ) ) ).Verifiable();

			var vm = new MainViewModel( ConnectorMock.Object, null, TaskRunnerMock.Object, null, messengerMock.Object, projectListMock.Object );

			// Act
			vm.OpenTagManagementCommand.Execute( null );

			// Assert
			messengerMock.VerifyAll();
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ProjectsCommandSendsMessage()
		{
			// Arrange
			var messengerMock = new Mock<IMessenger>();
			messengerMock.Setup( x => x.Send( It.Is<FlyoutMessage>( msg => msg.FlyoutName == "ProjectsFlyout" ) ) ).Verifiable();

			var vm = new MainViewModel( ConnectorMock.Object, null, TaskRunnerMock.Object, null, messengerMock.Object );

			// Act
			vm.OpenProjectManagementCommand.Execute( null );

			// Assert
			messengerMock.VerifyAll();
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ReportErrorSpawnsProcess()
		{
			// Arrange
			var procStarterMock = new Mock<IProcessStarter>();
			procStarterMock.Setup( x => x.Start( It.Is<string>( str => Uri.IsWellFormedUriString( str, UriKind.Absolute ) ) ) ).Verifiable();

			var vm = new MainViewModel( ConnectorMock.Object, null, TaskRunnerMock.Object, procStarterMock.Object );

			// Act
			vm.ErrorReportCommand.Execute( null );

			// Assert
			procStarterMock.VerifyAll();
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void SettingsCommandSendsMessage()
		{
			// Arrange
			var messengerMock = new Mock<IMessenger>();
			messengerMock.Setup( x => x.Send( It.Is<FlyoutMessage>( msg => msg.FlyoutName == "SettingsFlyout" ) ) ).Verifiable();

			var vm = new MainViewModel( ConnectorMock.Object, null, TaskRunnerMock.Object, null, messengerMock.Object );

			// Act
			vm.OpenSettingsCommand.Execute( null );

			// Assert
			messengerMock.VerifyAll();
		}

		private static Mock<IDatabaseConnector> ConnectorMock;
		private static Mock<ITaskRunner> TaskRunnerMock;
		private static Mock<IViewServiceRepository> ViewServiceRepoMock;
	}
}