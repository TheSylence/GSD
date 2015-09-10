using System.Linq;
using GSD.Models;
using GSD.Models.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GSD.Tests.Models.Repositories
{
	[TestClass]
	public class TagRepositoryTests : RepositoryTestBase
	{
		[ClassInitialize]
		public static void _ClassInit( TestContext context )
		{
			MapAction = map => map.FluentMappings.AddFromAssemblyOf<Tag>();

			ClassInitStatic();
		}

		[TestCleanup]
		public void _AfterTest()
		{
			AfterTest();
		}

		[TestInitialize]
		public void _BeforeTest()
		{
			BeforeTest();

			using( var tx = Session.BeginTransaction() )
			{
				Session.Save( TestProject );
				Session.Save( TestProject2 );

				foreach( var ent in TestData )
				{
					Session.Save( ent );
				}

				tx.Commit();
			}
		}

		[TestMethod, TestCategory( "Models.Repositories" )]
		public void GetAllNamesReturnsCorrectNames()
		{
			// Arrange
			ITagRepository repo = new TagRepository( Session );

			// Act
			string[] one = repo.GetAllNames( TestProject ).ToArray();
			string[] two = repo.GetAllNames( TestProject2 ).ToArray();

			// Assert
			CollectionAssert.AreEquivalent( TestData.Where( t => t.Project.Id == TestProject.Id ).Select( t => t.Name ).ToArray(), one );
			CollectionAssert.AreEquivalent( TestData.Where( t => t.Project.Id == TestProject2.Id ).Select( t => t.Name ).ToArray(), two );
		}

		private static readonly Project TestProject = new Project {Id = 1, Name = "TestProject"};
		private static readonly Project TestProject2 = new Project {Id = 2, Name = "TestProject"};

		private readonly Tag[] TestData =
		{
			new Tag {Id = 1, Name = "one", Project = TestProject},
			new Tag {Id = 2, Name = "two", Project = TestProject},
			new Tag {Id = 3, Name = "three", Project = TestProject},
			new Tag {Id = 3, Name = "three", Project = TestProject2}
		};
	}
}