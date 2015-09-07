using GSD.Messages;
using GSD.Models;
using GSD.Models.Repositories;
using GSD.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GSD.Tests.ViewModels
{
	[TestClass]
	public class ProjectListViewModelTests
	{
		[TestMethod, TestCategory( "ViewModels" )]
		public void CurrentProjectIsFirstIfNotSetInSettings()
		{
			// Arrange
			var context = new MockContext();
			context.SettingsRepoMock.Setup( s => s.GetById( SettingKeys.LastProject ) ).Returns( new Config {Value = "-1"} );

			var projects = new[]
			{
				new Project {Name = "One", Id = 1},
				new Project {Name = "Two", Id = 2},
				new Project {Name = "Three", Id = 3}
			};

			context.ProjectRepoMock.Setup( p => p.GetAll() ).Returns( projects );

			// Act
			var vm = new ProjectListViewModel( context.SettingsRepo, context.ProjectRepo );

			// Assert
			Assert.IsNotNull( vm.CurrentProject );
			Assert.AreSame( projects[0], vm.CurrentProject.Model );

			context.SettingsRepoMock.VerifyAll();
			context.ProjectRepoMock.VerifyAll();
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void CurrentProjectIsLastUsed()
		{
			// Arrange
			var context = new MockContext();
			context.SettingsRepoMock.Setup( s => s.GetById( SettingKeys.LastProject ) ).Returns( new Config {Value = "2"} );

			var projects = new[]
			{
				new Project {Name = "One", Id = 1},
				new Project {Name = "Two", Id = 2},
				new Project {Name = "Three", Id = 3}
			};

			context.ProjectRepoMock.Setup( p => p.GetAll() ).Returns( projects );

			// Act
			var vm = new ProjectListViewModel( context.SettingsRepo, context.ProjectRepo );

			// Assert
			Assert.IsNotNull( vm.CurrentProject );
			Assert.AreSame( projects[1], vm.CurrentProject.Model );

			context.SettingsRepoMock.VerifyAll();
			context.ProjectRepoMock.VerifyAll();
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void CurrentProjectMessageSavesIdToSettings()
		{
			// Arrange
			var context = new MockContext();
			context.SettingsRepoMock.Setup( s => s.Set( SettingKeys.LastProject, "1" ) ).Verifiable();

			var vm = new ProjectListViewModel( context.SettingsRepo, context.ProjectRepo );
			vm.Reset();

			vm.Projects.Add( new ProjectViewModel( new Project {Id = 1} ) {IsCurrent = true} );

			// Act
			vm.CurrentProject = vm.Projects[0];
			vm.TestMessenger.Send( new CurrentProjectChangedMessage() );

			// Assert
			context.SettingsRepoMock.VerifyAll();
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void DeleteProjectCommandNeedsValue()
		{
			// Arrange
			var context = new MockContext();
			var vm = new ProjectListViewModel( context.SettingsRepo, context.ProjectRepo );

			// Act
			bool withoutValue = vm.DeleteProjectCommand.CanExecute( null );
			bool withValue = vm.DeleteProjectCommand.CanExecute( new ProjectViewModel( new Project() ) );

			// Assert
			Assert.IsFalse( withoutValue );
			Assert.IsTrue( withValue );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void NewProjectCanOnlyBeAddedWithValidName()
		{
			// Arrange
			var context = new MockContext();
			var vm = new ProjectListViewModel( context.SettingsRepo, context.ProjectRepo );

			// Act
			vm.NewProjectName = string.Empty;
			bool empty = vm.NewProjectCommand.CanExecute( null );

			vm.NewProjectName = "test";
			bool nonEmpty = vm.NewProjectCommand.CanExecute( null );

			// Assert
			Assert.IsFalse( empty );
			Assert.IsTrue( nonEmpty );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ResetErasesProjectName()
		{
			// Arrange
			var context = new MockContext();
			var vm = new ProjectListViewModel( context.SettingsRepo, context.ProjectRepo );

			// Act
			vm.NewProjectName = "test";
			vm.Reset();

			// Assert
			Assert.IsTrue( string.IsNullOrEmpty( vm.NewProjectName ) );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void SettingCurrentProjectRaisesEvent()
		{
			// Arrange
			var context = new MockContext();
			var vm = new ProjectListViewModel( context.SettingsRepo, context.ProjectRepo );

			bool raised = false;
			vm.CurrentProjectChanged += ( s, e ) => raised = true;

			// Act
			vm.CurrentProject = new ProjectViewModel( new Project() );

			// Assert
			Assert.IsTrue( raised );
		}
	}
}