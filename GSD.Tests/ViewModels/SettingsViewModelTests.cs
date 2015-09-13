using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GSD.Models;
using GSD.Models.Repositories;
using GSD.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GSD.Tests.ViewModels
{
	[TestClass]
	public class SettingsViewModelTests
	{
		[TestInitialize]
		public void _BeforeTest()
		{
			MockContext = new MockContext();

			Settings =
			SettingKeys.DefaultValues.ToDictionary( kvp => kvp.Key, kvp => new Config { Id = kvp.Key, Value = kvp.Value } );

			MockContext.SettingsRepoMock.Setup( x => x.Set( It.IsAny<string>(), It.IsAny<string>() ) ).Callback<string, string>( ( key, value ) =>
			{
				Settings[key] = new Config { Id = key, Value = value };
			} );
			MockContext.SettingsRepoMock.Setup( x => x.GetById( It.IsAny<string>() ) ).Returns<string>( cfg => Settings[cfg] );

			MockContext.AppThemesMock.SetupGet( x => x.Accents ).Returns( new[]
			{
				new ColorItem {Name = "Red"},
				new ColorItem {Name = "Blue"},
				new ColorItem {Name = "Green"}
			} );
			MockContext.AppThemesMock.SetupGet( x => x.Themes ).Returns( new[]
			{
				new ColorItem {Name = "BaseDark"},
				new ColorItem {Name = "None"},
				new ColorItem {Name = "BaseLight"}
			} );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ResetToDefaultsSetsDefaultValues()
		{
			// Arrange
			var vm = new SettingsViewModel( MockContext.ViewServiceRepo, MockContext.SettingsRepo, MockContext.AppThemes );

			// Act
			vm.SelectedTheme = new ColorItem { Name = "test" };
			vm.SelectedAccent = new ColorItem { Name = "test" };
			vm.SelectedLanguage = CultureInfo.CreateSpecificCulture( "es-ES" );

			vm.ResetToDefaultsCommand.Execute( null );

			// Assert
			Assert.AreEqual( SettingKeys.DefaultValues[SettingKeys.Accent], vm.SelectedAccent.Name );
			Assert.AreEqual( SettingKeys.DefaultValues[SettingKeys.Theme], vm.SelectedTheme.Name );
			Assert.AreEqual( SettingKeys.DefaultValues[SettingKeys.Language], vm.SelectedLanguage.IetfLanguageTag );
			Assert.AreEqual( SettingKeys.DefaultValues[SettingKeys.DatabasePath], vm.DatabasePath );
			Assert.AreEqual( SettingKeys.DefaultValues[SettingKeys.ExpandEntries], vm.ExpandEntries.ToString() );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void SaveSetsValues()
		{
			// Arrange
			var vm = new SettingsViewModel( MockContext.ViewServiceRepo, MockContext.SettingsRepo, MockContext.AppThemes );

			// Act
			vm.SelectedAccent = vm.AvailableAccents.Last();
			vm.SelectedTheme = vm.AvailableThemes.Last();
			vm.SelectedLanguage = vm.AvailableLanguages.Last();
			vm.ExpandEntries = !vm.ExpandEntries;

			vm.SaveCommand.Execute( null );

			// Assert
			Assert.AreEqual( vm.AvailableAccents.Last().Name, Settings[SettingKeys.Accent].Value );
			Assert.AreEqual( vm.AvailableThemes.Last().Name, Settings[SettingKeys.Theme].Value );
			Assert.AreEqual( vm.AvailableLanguages.Last().IetfLanguageTag, Settings[SettingKeys.Language].Value );
			Assert.AreEqual( vm.ExpandEntries.ToString(), Settings[SettingKeys.ExpandEntries].Value );
		}

		private MockContext MockContext;
		private Dictionary<string, Config> Settings;
	}
}