using GSD.Messages;
using GSD.Models;
using GSD.Models.Repositories;
using GSD.ViewModels;
using GSD.ViewServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace GSD.Tests.ViewModels
{
	[TestClass]
	public class ProjectListViewModelTests
	{
		[TestMethod, TestCategory( "ViewModels" )]
		public void AddTagAddsToProject()
		{
			// Arrange
			var context = new MockContext();

			context.ProjectRepoMock.Setup( x => x.Update( It.Is<Project>( p => p.Id == 1 ) ) );
			context.ProjectRepoMock.Setup( x => x.GetAll() ).Returns( Enumerable.Empty<Project>() );
			context.SettingsRepoMock.Setup( x => x.GetById( "state.project" ) ).Returns( new Config { Id = "-1" } );

			var vm = new ProjectListViewModel( context.ViewServiceRepo, context.SettingsRepo, context.ProjectRepo )
			{
				CurrentProject = new ProjectViewModel( new Project { Id = 1 } )
			};

			// Act
			var tag = new Tag { Id = 123 };
			vm.AddTag( tag );

			// Assert
			Assert.IsNotNull( vm.CurrentProject.Model.Tags.SingleOrDefault( t => t.Id == tag.Id ) );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void CurrentProjectIsFirstIfNotSetInSettings()
		{
			// Arrange
			var context = new MockContext();
			context.SettingsRepoMock.Setup( s => s.GetById( SettingKeys.LastProject ) ).Returns( new Config { Value = "-1" } );

			var projects = new[]
			{
				new Project {Name = "One", Id = 1},
				new Project {Name = "Two", Id = 2},
				new Project {Name = "Three", Id = 3}
			};

			context.ProjectRepoMock.Setup( p => p.GetAll() ).Returns( projects );

			// Act
			var vm = new ProjectListViewModel( context.ViewServiceRepo, context.SettingsRepo, context.ProjectRepo );

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
			context.SettingsRepoMock.Setup( s => s.GetById( SettingKeys.LastProject ) ).Returns( new Config { Value = "2" } );

			var projects = new[]
			{
				new Project {Name = "One", Id = 1},
				new Project {Name = "Two", Id = 2},
				new Project {Name = "Three", Id = 3}
			};

			context.ProjectRepoMock.Setup( p => p.GetAll() ).Returns( projects );

			// Act
			var vm = new ProjectListViewModel( context.ViewServiceRepo, context.SettingsRepo, context.ProjectRepo );

			// Assert
			Assert.IsNotNull( vm.CurrentProject );
			Assert.AreSame( projects[1], vm.CurrentProject.Model );

			context.SettingsRepoMock.VerifyAll();
			context.ProjectRepoMock.VerifyAll();
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ChangingCurrentProjectSavesToSettings()
		{
			// Arrange
			var context = new MockContext();
			context.SettingsRepoMock.Setup( s => s.Set( SettingKeys.LastProject, "1" ) ).Verifiable();
			context.SettingsRepoMock.Setup( x => x.GetById( SettingKeys.LastProject ) ).Returns( new Config {Id = SettingKeys.LastProject, Value = "-1"} );
			context.ProjectRepoMock.Setup( x => x.GetAll() ).Returns( new[] { new Project {Id = 1} } );

			var vm = new ProjectListViewModel( context.ViewServiceRepo, context.SettingsRepo, context.ProjectRepo );
			vm.Reset();

			// Act
			vm.Projects[0].IsCurrent = true;

			// Assert
			context.SettingsRepoMock.VerifyAll();
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void DeleteProjectCommandNeedsValue()
		{
			// Arrange
			var context = new MockContext();
			var vm = new ProjectListViewModel( context.ViewServiceRepo, context.SettingsRepo, context.ProjectRepo );

			// Act
			bool withoutValue = vm.DeleteProjectCommand.CanExecute( null );
			bool withValue = vm.DeleteProjectCommand.CanExecute( new ProjectViewModel( new Project() ) );

			// Assert
			Assert.IsFalse( withoutValue );
			Assert.IsTrue( withValue );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void DeleteProjectNeedsConfirmation()
		{
			// Arrange
			var context = new MockContext();
			var projects = new[]
			{
				new Project {Name = "One", Id = 1},
				new Project {Name = "Two", Id = 2},
				new Project {Name = "Three", Id = 3}
			};

			context.SettingsRepoMock.Setup( s => s.GetById( SettingKeys.LastProject ) ).Returns( new Config { Value = "-1" } );
			context.ProjectRepoMock.Setup( p => p.GetAll() ).Returns( projects );

			context.ViewServiceRepoMock.Setup( x => x.Execute<IConfirmationService, bool>( It.IsAny<ConfirmationServiceArgs>() ) )
				.Returns( Task.FromResult( false ) );

			var vm = new ProjectListViewModel( context.ViewServiceRepo, context.SettingsRepo, context.ProjectRepo );

			// Act
			vm.DeleteProjectCommand.Execute( vm.Projects.First() );

			// Assert
			context.ViewServiceRepoMock.VerifyAll();
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void DeleteProjectRemovesFromListAndRepository()
		{
			// Arrange
			var context = new MockContext();
			var projects = new[]
			{
				new Project {Name = "One", Id = 1},
				new Project {Name = "Two", Id = 2},
				new Project {Name = "Three", Id = 3}
			};

			context.SettingsRepoMock.Setup( s => s.GetById( SettingKeys.LastProject ) ).Returns( new Config { Value = "-1" } );

			context.ProjectRepoMock.Setup( p => p.GetAll() ).Returns( projects );
			context.ProjectRepoMock.Setup( p => p.Delete( It.Is<Project>( proj => proj.Id == 1 ) ) ).Verifiable();

			context.ViewServiceRepoMock.Setup( x => x.Execute<IConfirmationService, bool>( It.IsAny<ConfirmationServiceArgs>() ) )
				.Returns( Task.FromResult( true ) );

			var vm = new ProjectListViewModel( context.ViewServiceRepo, context.SettingsRepo, context.ProjectRepo );

			// Act
			vm.DeleteProjectCommand.Execute( vm.Projects.First() );

			// Assert
			Assert.IsNull( vm.Projects.FirstOrDefault( p => p.Model.Id == 1 ) );

			context.ViewServiceRepoMock.VerifyAll();
			context.ProjectRepoMock.VerifyAll();
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void NewProjectCanOnlyBeAddedWithValidName()
		{
			// Arrange
			var context = new MockContext();
			var vm = new ProjectListViewModel( context.ViewServiceRepo, context.SettingsRepo, context.ProjectRepo );

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
			var vm = new ProjectListViewModel( context.ViewServiceRepo, context.SettingsRepo, context.ProjectRepo );

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
			var vm = new ProjectListViewModel( context.ViewServiceRepo, context.SettingsRepo, context.ProjectRepo );

			bool raised = false;
			vm.CurrentProjectChanged += ( s, e ) => raised = true;

			// Act
			vm.CurrentProject = new ProjectViewModel( new Project() );

			// Assert
			Assert.IsTrue( raised );
		}
	}
}