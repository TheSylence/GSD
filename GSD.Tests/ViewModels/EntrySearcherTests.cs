using GalaSoft.MvvmLight.Messaging;
using GSD.Messages;
using GSD.Models;
using GSD.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace GSD.Tests.ViewModels
{
	[TestClass]
	public class EntrySearcherTests
	{
		[TestInitialize]
		public void _BeforeTest()
		{
			Tags = new[]
			{
				new Tag {Name = "Tag1"},
				new Tag {Name = "Tag2"},
				new Tag {Name = "Tag3"}
			};

			Todos = new[]
			{
				new Todo {Summary = "Test"},
				new Todo {Summary = "abc", Details = "details"},
				new Todo {Summary = "123", Done = true}
			};

			Todos[0].Tags.Add( Tags[1] );
			Todos[1].Tags.Add( Tags[1] );
			Todos[1].Tags.Add( Tags[2] );

			Project = new Project();
			foreach( var t in Tags )
			{
				Project.Tags.Add( t );
			}

			foreach( var t in Todos )
			{
				Project.Todos.Add( t );
			}

			ProjectVm = new ProjectViewModel( Project );

			var mock = new Mock<IProjectListViewModel>();
			mock.SetupGet( x => x.CurrentProject ).Returns( ProjectVm );
			ProjectListVm = mock.Object;
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void CurrentProjectIsChangedOnMessage()
		{
			// Arrange
			var messenger = new Messenger();

			var projectListMock = new Mock<IProjectListViewModel>();
			projectListMock.SetupGet( x => x.CurrentProject ).Verifiable();

			var searcher = new EntrySearcher( projectListMock.Object, messenger );

			// Act
			messenger.Send( new CurrentProjectChangedMessage() );

			// Assert
			projectListMock.VerifyGet( x => x.CurrentProject, Times.Exactly( 2 ) );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void IsSearchingPropertyIsCorrectlySet()
		{
			// Arrange
			var projectListMock = new Mock<IProjectListViewModel>();
			var searcher = new EntrySearcher( projectListMock.Object );

			// Act
			searcher.Text = string.Empty;
			bool empty = searcher.IsSearching;

			searcher.Text = "test";
			bool nonEmpty = searcher.IsSearching;

			// Assert
			Assert.IsFalse( empty );
			Assert.IsTrue( nonEmpty );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void SearchForDetailsReturnsMatchingEntries()
		{
			// Arrange
			var searcher = new EntrySearcher( ProjectListVm );

			// Act
			searcher.Text = "details:det";

			// Assert
			Assert.AreEqual( 1, searcher.Matches.Count );
			Assert.AreSame( Todos[1], searcher.Matches.First().Model );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void SearchForSummaryReturnsMatchingEntries()
		{
			// Arrange
			var searcher = new EntrySearcher( ProjectListVm );

			// Act
			searcher.Text = "test";

			// Assert
			Assert.AreEqual( 1, searcher.Matches.Count );
			Assert.AreSame( Todos[0], searcher.Matches.First().Model );
		}

		private Project Project;
		private IProjectListViewModel ProjectListVm;
		private ProjectViewModel ProjectVm;
		private Tag[] Tags;
		private Todo[] Todos;
	}
}