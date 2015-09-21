using GalaSoft.MvvmLight.Messaging;
using GSD.Messages;
using GSD.Models;
using GSD.Models.Repositories;
using GSD.Resources;
using GSD.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace GSD.Tests.ViewModels
{
	[TestClass]
	public class AddEntryViewModelTests
	{
		[TestMethod, TestCategory( "ViewModels" )]
		public void AddCommandNeedsNonEmptySummary()
		{
			// Arrange
			var vm = new AddEntryViewModel( Enumerable.Empty<TagViewModel>(), null );

			// Act
			vm.Summary = null;
			bool empty = vm.AddCommand.CanExecute( null );
			vm.Summary = "test";
			bool nonEmpty = vm.AddCommand.CanExecute( null );

			// Assert
			Assert.IsFalse( empty );
			Assert.IsTrue( nonEmpty );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void AddUsesCorrectValues()
		{
			// Arrange
			var tags = new[]
			{
				new TagViewModel( new Tag {Id = 1, Name = "Tag1"} ),
				new TagViewModel( new Tag {Id = 2, Name = "Tag2"} )
			};

			var currentProject = new ProjectViewModel( new Project { Id = 123 } );

			var messenger = new Messenger();
			bool addMessageReceived = false;
			messenger.Register<EntryAddedMessage>( this, msg => addMessageReceived = true );

			var todoRepoMock = new Mock<ITodoRepository>();
			todoRepoMock.Setup( x => x.Add( It.Is<Todo>( t => t.Summary.Equals( "summary" ) && t.Details.Equals( "details" ) && t.Project.Id == 123 ) ) ).Verifiable();
			todoRepoMock.Setup( x => x.Update( It.Is<Todo>( t => t.Tags.First().Id == 2 ) ) ).Verifiable();

			var vm = new AddEntryViewModel( tags, currentProject, todoRepoMock.Object, messenger )
			{
				Summary = "summary",
				Details = "details"
			};

			// Act
			vm.Tags[1].IsSelected = true;
			vm.AddCommand.Execute( null );

			// Assert
			todoRepoMock.VerifyAll();
			Assert.IsTrue( addMessageReceived );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void FlyoutStaysOpenIfWanted()
		{
			// Arrange
			var currentProject = new ProjectViewModel( new Project { Id = 123 } );
			var messenger = new Messenger();
			var todoRepoMock = new Mock<ITodoRepository>();

			bool messageReceived = false;
			messenger.Register<FlyoutMessage>( this, msg => { if( msg.FlyoutName == FlyoutMessage.AddEntryFlyoutName ) messageReceived = true; } );

			var vm = new AddEntryViewModel( Enumerable.Empty<TagViewModel>(), currentProject, todoRepoMock.Object, messenger )
			{
				Summary = "summary"
			};

			// Act
			vm.StayOpen = true;
			vm.AddCommand.Execute( null );
			bool stay = messageReceived;

			vm.Summary = "summary";
			vm.StayOpen = false;
			vm.AddCommand.Execute( null );
			bool dontStay = messageReceived;

			// Assert
			Assert.IsFalse( stay );
			Assert.IsTrue( dontStay );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ResetClearsTagSelection()
		{
			// Arrange
			var tags = new[]
			{
				new TagViewModel( new Tag {Id = 1, Name = "Tag1"} ),
				new TagViewModel( new Tag {Id = 2, Name = "Tag2"} )
			};

			var vm = new AddEntryViewModel( tags, null );

			// Act
			vm.Tags[1].IsSelected = true;
			vm.Reset();

			// Assert
			Assert.IsTrue( vm.Tags.All( t => !t.IsSelected ) );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ResetClearsTextFields()
		{
			// Arrange
			var vm = new AddEntryViewModel( Enumerable.Empty<TagViewModel>(), null );

			// Act
			vm.Summary = "test";
			vm.Details = "test";
			vm.Reset();

			// Assert
			Assert.IsTrue( string.IsNullOrEmpty( vm.Summary ) );
			Assert.IsTrue( string.IsNullOrEmpty( vm.Details ) );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void SummaryIsValidatedForNonEmpty()
		{
			// Arrange
			var vm = new AddEntryViewModel( Enumerable.Empty<TagViewModel>(), null );

			// Act
			var start = vm.GetErrors( nameof( AddEntryViewModel.Summary ) ).Cast<string>().ToArray();
			vm.Summary = "test";
			var valid = vm.GetErrors( nameof( AddEntryViewModel.Summary ) ).Cast<string>().ToArray();
			vm.Summary = "";
			var empty = vm.GetErrors( nameof( AddEntryViewModel.Summary ) ).Cast<string>().ToArray();

			// Assert
			Assert.IsFalse( start.Any() );
			Assert.IsFalse( valid.Any() );
			CollectionAssert.AreEqual( new[] { Strings.EntryNeedsSummary }, empty );
		}
	}
}