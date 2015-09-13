using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Tool.hbm2ddl;

namespace GSD.Tests.Models
{
	[TestClass]
	public class EntityTests
	{
		[TestMethod, TestCategory( "Models" )]
		public void EqualsNullIsFalse()
		{
			// Arrange
			var entity = new TestEntity();

			// Act
			bool equals = entity.Equals( null );

			// Assert
			Assert.IsFalse( equals );
		}

		[TestMethod, TestCategory( "Models" )]
		public void EqualsOtherTypeIsFalse()
		{
			// Arrange
			var entity = new TestEntity();

			// Act
			// ReSharper disable once SuspiciousTypeConversion.Global
			bool equals = entity.Equals( string.Empty );

			// Assert
			Assert.IsFalse( equals );
		}

		[TestMethod, TestCategory( "Models" )]
		public void EqualsWithNewObjectsUsesReferenceEquals()
		{
			// Arrange
			var ent1 = new TestEntity();
			var ent2 = new TestEntity();

			// Act
			bool different = ent1.Equals( ent2 );
			bool different2 = ent2.Equals( ent1 );
			bool different3 = ent1 == ent2;
			bool equal3 = ent1 != ent2;

			var ent3 = ent1;
			bool equal = ent1.Equals( ent3 );
			bool equal2 = ent3.Equals( ent1 );
			bool equal4 = ent1 == ent3;
			bool different4 = ent1 != ent3;

			// Assert
			Assert.IsFalse( different );
			Assert.IsFalse( different2 );
			Assert.IsFalse( different3 );
			Assert.IsFalse( different4 );
			Assert.IsTrue( equal );
			Assert.IsTrue( equal2 );
			Assert.IsTrue( equal3 );
			Assert.IsTrue( equal4 );
		}

		[TestMethod, TestCategory( "Models" )]
		public void EqualsWorksWithSavedObjects()
		{
			// Arrange
			var dbConfig = SQLiteConfiguration.Standard.InMemory().ShowSql();

			var config = Fluently.Configure().Database( dbConfig )
				.Mappings( map => map.FluentMappings.Add<TestEntityMap>() )
				.BuildConfiguration();

			var ent1 = new TestEntity { PropTwo = 1 };
			var ent2 = new TestEntity { PropTwo = 2 };

			var sessionFactory = config.BuildSessionFactory();
			using( var session = sessionFactory.OpenSession() )
			{
				new SchemaExport( config ).Execute( true, true, false, session.Connection, null );

				using( var tx = session.BeginTransaction() )
				{
					session.Save( ent1 );
					session.Save( ent2 );

					tx.Commit();
				}
			}

			// Act
			bool different = ent1.Equals( ent2 );
			bool differnt2 = ent2.Equals( ent1 );

			var ent3 = ent1;
			bool equal = ent3.Equals( ent1 );
			bool equal2 = ent1.Equals( ent3 );

			// Assert
			Assert.IsFalse( different );
			Assert.IsFalse( differnt2 );
			Assert.IsTrue( equal );
			Assert.IsTrue( equal2 );
		}

		[TestMethod, TestCategory( "Models" )]
		public void EqualsWorksWithUnsavedObjects()
		{
			// Arrange
			TestEntity ent1 = new TestEntity { Id = 1 };
			TestEntity ent2 = new TestEntity { Id = 2 };

			// Act
			bool different = ent1.Equals( ent2 );
			bool different2 = ent2.Equals( ent1 );

			ent1.Id = 2;
			bool equal = ent1.Equals( ent2 );
			bool equal2 = ent2.Equals( ent1 );

			// Assert
			Assert.IsFalse( different );
			Assert.IsFalse( different2 );
			Assert.IsTrue( equal );
			Assert.IsTrue( equal2 );
		}

		[TestMethod, TestCategory( "Models" )]
		public void HashCodeForSavedIsDifferentForId()
		{
			// Arrange
			var ent = new TestEntity { Id = 123 };

			// Act
			int hash1 = ent.GetHashCode();
			ent.Id = 444;
			int hash2 = ent.GetHashCode();

			// Assert
			Assert.AreNotEqual( hash1, hash2 );
		}

		[TestMethod, TestCategory( "Models" )]
		public void HashCodeNeverChanges()
		{
			// Arrange
			var ent = new TestEntity();

			// Act
			int hash1 = ent.GetHashCode();
			ent.Id = 123;
			int hash2 = ent.GetHashCode();

			// Assert
			Assert.AreEqual( hash1, hash2 );
		}
	}
}