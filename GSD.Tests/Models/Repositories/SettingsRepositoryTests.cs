using GSD.Models.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace GSD.Tests.Models.Repositories
{
	[TestClass]
	public class SettingsRepositoryTests
	{
		[TestMethod, TestCategory( "Models.Repositories" )]
		public void AddIsNotSupported()
		{
			// Arrange
			ISettingsRepository repo = new SettingsRepository( "non.existing.file" );

			// Act

			// Assert
			ExceptionAssert.Throws<NotSupportedException>( () => repo.Add( new GSD.Models.Config() ) );
		}

		[TestMethod, TestCategory( "Models.Repositories" )]
		public void ConstructFillsMissingKeys()
		{
			string fileName = GetTempFileName();
			var sb = new StringBuilder();

			foreach( var kvp in SettingKeys.DefaultValues.Skip( 1 ) )
			{
				sb.AppendLine( $"{kvp.Key}={kvp.Key}" );
			}

			File.WriteAllText( fileName, sb.ToString() );

			// Act
			ISettingsRepository repo = new SettingsRepository( fileName );
			var keys = repo.GetAll().Select( c => c.Id ).ToArray();

			// Assert
			CollectionAssert.AreEquivalent( SettingKeys.DefaultValues.Keys.ToArray(), keys );
		}

		[TestMethod, TestCategory( "Models.Repositories" )]
		public void ConstructReadsValueForExistingFile()
		{
			// Arrange
			string fileName = GetTempFileName();
			var sb = new StringBuilder();

			foreach( var kvp in SettingKeys.DefaultValues )
			{
				sb.AppendLine( $"{kvp.Key}={kvp.Key}" );
			}

			File.WriteAllText( fileName, sb.ToString() );

			// Act
			ISettingsRepository repo = new SettingsRepository( fileName );

			// Assert
			foreach( var kvp in SettingKeys.DefaultValues )
			{
				Assert.AreEqual( kvp.Key, repo.GetById( kvp.Key ).Value );
			}
		}

		[TestMethod, TestCategory( "Models.Repositories" )]
		public void ConstructSetsDefaultValuesForNonExistingFile()
		{
			// Arrange

			// Act
			ISettingsRepository repo = new SettingsRepository( "non.existing.file" );

			// Assert
			foreach( var kvp in SettingKeys.DefaultValues )
			{
				Assert.AreEqual( kvp.Value, repo.GetById( kvp.Key ).Value );
			}
		}

		[TestMethod, TestCategory( "Models.Repositories" )]
		public void DeleteIsNotSupported()
		{
			// Arrange
			ISettingsRepository repo = new SettingsRepository( "non.existing.file" );

			// Act

			// Assert
			ExceptionAssert.Throws<NotSupportedException>( () => repo.Delete( new GSD.Models.Config() ) );
		}

		[TestMethod, TestCategory( "Models.Repositories" )]
		public void GetAllReturnsCorrectValues()
		{
			// Arrange
			ISettingsRepository repo = new SettingsRepository( "non.existing.file" );

			// Act
			var keys = repo.GetAll().Select( c => c.Id ).ToArray();

			// Assert
			CollectionAssert.AreEquivalent( SettingKeys.DefaultValues.Keys.ToArray(), keys );
		}

		[TestMethod, TestCategory( "Models.Repositories" )]
		public void SetNullSetsDefaultValue()
		{
			// Arrange
			string fileName = GetTempFileName();
			ISettingsRepository repo = new SettingsRepository( fileName );

			// Act
			repo.Set( SettingKeys.Accent, null );

			// Assert
			Assert.IsTrue( File.ReadAllText( fileName ).Contains( $"{SettingKeys.Accent}={SettingKeys.DefaultValues[SettingKeys.Accent]}" ) );
		}

		[TestMethod, TestCategory( "Models.Repositories" )]
		public void SetWritesNewValue()
		{
			// Arrange
			string fileName = GetTempFileName();
			ISettingsRepository repo = new SettingsRepository( fileName );

			// Act
			repo.Set( SettingKeys.Theme, "test" );

			// Assert
			Assert.IsTrue( File.ReadAllText( fileName ).Contains( $"{SettingKeys.Theme}=test" ) );
		}

		private static string GetTempFileName()
		{
			return Path.GetTempFileName() + ".test";
		}
	}
}