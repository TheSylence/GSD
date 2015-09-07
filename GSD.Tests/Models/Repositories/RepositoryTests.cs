using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Cfg;

namespace GSD.Tests.Models.Repositories
{
	[TestClass]
	public class RepositoryTests : RepositoryTestBase
	{
		[ClassInitialize]
		public static void _ClassInit( TestContext context )
		{
			ClassInitStatic( context );
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
				foreach( var ent in TestData )
				{
					Session.Save( ent );
				}

				tx.Commit();
			}
		}

		[TestMethod, TestCategory( "Models.Repositories" )]
		public void CanAddEntity()
		{
			// Arrange
			var entity = new TestEntity {Id = 123, PropOne = "One", PropTwo = 2};
			var repo = new TestRepository( Session );

			// Act
			repo.Add( entity );

			// Assert

			var fromDb = Session.Get<TestEntity>( entity.Id );

			Assert.IsNotNull( fromDb );
			Assert.AreEqual( entity.PropOne, fromDb.PropOne );
			Assert.AreEqual( entity.PropTwo, fromDb.PropTwo );
		}

		[TestMethod, TestCategory( "Models.Repositories" )]
		public void CanGetAllEntities()
		{
			// Arrange
			var repo = new TestRepository( Session );

			// Act
			var all = repo.GetAll().ToArray();

			// Assert
			Assert.AreEqual( TestData.Length, all.Length );
		}

		[TestMethod, TestCategory( "Models.Repositories" )]
		public void CanGetExistingEntity()
		{
			// Arrange
			var repo = new TestRepository( Session );

			// Act
			var fromDb = repo.GetById( TestData[2].Id );

			// Assert
			Assert.IsNotNull( fromDb );
			Assert.AreEqual( TestData[2].PropOne, fromDb.PropOne );
			Assert.AreEqual( TestData[2].PropTwo, fromDb.PropTwo );
		}

		[TestMethod, TestCategory( "Models.Repositories" )]
		public void CanRemoveEntitiy()
		{
			// Arrange
			var entity = TestData[0];
			var repo = new TestRepository( Session );

			// Act
			repo.Delete( entity );

			// Assert
			var fromDb = Session.Get<TestEntity>( entity.Id );
			Assert.IsNull( fromDb );
		}

		[TestMethod, TestCategory( "Models.Repositories" )]
		public void CanUpdateExisting()
		{
			// Arrange
			var entity = TestData[1];
			var repo = new TestRepository( Session );

			// Act
			entity.PropOne = "Hello World";
			repo.Update( entity );

			// Assert
			var fromDb = Session.Get<TestEntity>( entity.Id );
			Assert.AreEqual( entity.PropOne, fromDb.PropOne );
		}

		private readonly TestEntity[] TestData =
		{
			new TestEntity {PropOne = "One", PropTwo = 1},
			new TestEntity {PropOne = "Two", PropTwo = 2},
			new TestEntity {PropOne = "Three", PropTwo = 3}
		};
	}
}