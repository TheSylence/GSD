using System.IO;
using GSD.Models;
using GSD.Models.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GSD.Tests.Models.Repositories
{
	[TestClass]
	public class SettingsRepositoryTests
	{
		[TestMethod, TestCategory( "Models.Repositories" )]
		public void SetNullSetsDefaultValue()
		{
			// Arrange
			string fileName = Path.GetTempFileName() + ".test";
			ISettingsRepository repo = new SettingsRepository( fileName );

			// Act
			repo.Set( SettingKeys.Accent, null );

			// Assert
			Assert.IsTrue( File.ReadAllText( fileName ).Contains( $"{SettingKeys.Accent}={SettingKeys.DefaultValues[SettingKeys.Accent]}" ) );
		}

		[TestMethod, TestCategory( "Models.Repository" )]
		public void SetWritesNewValue()
		{
			// Arrange
			string fileName = Path.GetTempFileName() + ".test";
			ISettingsRepository repo = new SettingsRepository( fileName );

			// Act
			repo.Set( SettingKeys.Theme, "test" );

			// Assert
			Assert.IsTrue( File.ReadAllText( fileName ).Contains( $"{SettingKeys.Theme}=test" ) );
		}
	}
}