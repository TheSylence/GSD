using GSD.Models;
using GSD.Models.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GSD.Tests.Models.Repositories
{
	[TestClass]
	public class SettingsRepositoryTests : RepositoryTestBase
	{
		[ClassInitialize]
		public static void _ClassInit( TestContext context )
		{
			MapAction = map => map.FluentMappings.AddFromAssemblyOf<Config>();

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
		public void SetNullSetsDefaultValue()
		{
			// Arrange
			ISettingsRepository repo = new SettingsRepository( Session );

			// Act
			repo.Set( SettingKeys.Accent, null );

			// Assert
			var fromDb = Session.Get<Config>( SettingKeys.Accent );

			Assert.AreEqual( SettingKeys.DefaultValues[SettingKeys.Accent], fromDb.Value );
		}

		[TestMethod, TestCategory( "Models.Repository" )]
		public void SetWritesNewValue()
		{
			// Arrange
			ISettingsRepository repo = new SettingsRepository( Session );

			// Act
			repo.Set( SettingKeys.Theme, "test" );

			// Assert
			var fromDb = Session.Get<Config>( SettingKeys.Theme );
			Assert.AreEqual( "test", fromDb.Value );
		}

		private readonly Config[] TestData =
		{
			new Config {Id = SettingKeys.Accent, Value = "accent1"},
			new Config {Id = SettingKeys.LastProject, Value = "project1"},
			new Config {Id = SettingKeys.Theme, Value = "theme1"}
		};
	}
}