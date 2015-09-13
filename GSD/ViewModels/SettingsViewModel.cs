﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight.CommandWpf;
using GSD.Models.Repositories;
using GSD.ViewServices;
using MahApps.Metro;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;

namespace GSD.ViewModels
{
	internal class SettingsViewModel : ViewModelBaseEx, IResettable
	{
		public SettingsViewModel()
		{
			AvailableAccents = ThemeManager.Accents.Select( a => new ColorItem
			{
				Name = a.Name,
				BorderBrush = a.Resources["AccentColorBrush"] as Brush
			} ).ToList();

			AvailableThemes = ThemeManager.AppThemes.Select( t => new ColorItem
			{
				Name = t.Name,
				ColorBrush = t.Resources["WhiteColorBrush"] as Brush,
				BorderBrush = t.Resources["BlackColorBrush"] as Brush
			} ).ToList();

			ResxLocalizationProvider.Instance.UpdateCultureList( GetType().Assembly.FullName, "Strings" );
			IEnumerable<CultureInfo> languages = ResxLocalizationProvider.Instance.AvailableCultures;
			languages = languages.Where( l => !Equals( l, CultureInfo.InvariantCulture ) );

			AvailableLanguages = new List<CultureInfo>( languages.OrderBy( l => l.NativeName ) );
		}

		public void Reset()
		{
			var accent = Settings.GetById( SettingKeys.Accent ).Value;
			var theme = Settings.GetById( SettingKeys.Theme ).Value;

			SelectedAccent = AvailableAccents.FirstOrDefault( a => a.Name == accent );
			SelectedTheme = AvailableThemes.FirstOrDefault( t => t.Name == theme );

			ExpandEntries = Settings.GetById( SettingKeys.ExpandEntries ).Get<bool>();

			var lang = Settings.GetById( SettingKeys.Language ).Value;
			ChangeLanguage = false;
			SelectedLanguage = AvailableLanguages.FirstOrDefault( l => l.IetfLanguageTag.Equals( lang ) ) ??
								AvailableLanguages.FirstOrDefault( l => l.IetfLanguageTag.Equals( "en-US" ) );
			ChangeLanguage = true;

			var path = Settings.GetById( SettingKeys.DatabasePath )?.Value;
			if( string.IsNullOrWhiteSpace( path ) )
			{
				path = Constants.DefaultDatabasePath;
			}

			DatabasePath = path;
		}

		private async void ExecuteMoveDatabaseCommand()
		{
			var newPath = await ViewServices.Execute<IMoveDatabaseService, string>();
			if( string.IsNullOrWhiteSpace( newPath ) )
			{
				return;
			}
		}

		private void ExecuteOpenDatabaseFolderCommand()
		{
			var path = Path.GetDirectoryName( DatabasePath );
			if( path != null )
			{
				Process.Start( path );
			}
		}

		private void ExecuteResetToDefaultsCommand()
		{
			foreach( var key in new[] { SettingKeys.Accent, SettingKeys.Theme } )
			{
				Settings.Set( key, SettingKeys.DefaultValues[key] );
			}
		}

		private void ExecuteSaveCommand()
		{
			Settings.Set( SettingKeys.Accent, SelectedAccent.Name );
			Settings.Set( SettingKeys.Theme, SelectedTheme.Name );
			Settings.Set( SettingKeys.ExpandEntries, ExpandEntries.ToString() );
			Settings.Set( SettingKeys.Language, SelectedLanguage.IetfLanguageTag );

			var accent = ThemeManager.Accents.FirstOrDefault( a => a.Name == SelectedAccent.Name );
			var theme = ThemeManager.AppThemes.FirstOrDefault( t => t.Name == SelectedTheme.Name );

			ThemeManager.ChangeAppStyle( Application.Current, accent, theme );
		}

		public List<ColorItem> AvailableAccents { get; }

		public List<CultureInfo> AvailableLanguages { get; }

		public List<ColorItem> AvailableThemes { get; }

		public string DatabasePath
		{
			[DebuggerStepThrough]
			get
			{
				return _DatabasePath;
			}
			set
			{
				if( _DatabasePath == value )
				{
					return;
				}

				_DatabasePath = value;
				RaisePropertyChanged();
			}
		}

		public bool ExpandEntries
		{
			[DebuggerStepThrough]
			get
			{
				return _ExpandEntries;
			}
			set
			{
				if( _ExpandEntries == value )
				{
					return;
				}

				_ExpandEntries = value;
				RaisePropertyChanged();
			}
		}

		public RelayCommand MoveDatabaseCommand => _MoveDatabaseCommand ?? ( _MoveDatabaseCommand = new RelayCommand( ExecuteMoveDatabaseCommand ) );

		public RelayCommand OpenDatabaseFolderCommand => _OpenDatabaseFolderCommand ?? ( _OpenDatabaseFolderCommand = new RelayCommand( ExecuteOpenDatabaseFolderCommand ) );

		public RelayCommand ResetToDefaultsCommand => _ResetToDefaultsCommand ?? ( _ResetToDefaultsCommand = new RelayCommand( ExecuteResetToDefaultsCommand ) );

		public RelayCommand SaveCommand => _SaveCommand ?? ( _SaveCommand = new RelayCommand( ExecuteSaveCommand ) );

		public ColorItem SelectedAccent
		{
			[DebuggerStepThrough]
			get
			{
				return _SelectedAccent;
			}
			set
			{
				if( _SelectedAccent == value )
				{
					return;
				}

				_SelectedAccent = value;
				RaisePropertyChanged();
			}
		}

		public CultureInfo SelectedLanguage
		{
			[DebuggerStepThrough]
			get
			{
				return _SelectedLanguage;
			}
			set
			{
				if( Equals( _SelectedLanguage, value ) )
				{
					return;
				}

				_SelectedLanguage = value;
				RaisePropertyChanged();

				if( ChangeLanguage )
				{
					LocalizeDictionary.Instance.Culture = value;
					Thread.CurrentThread.CurrentUICulture = value;
					Thread.CurrentThread.CurrentCulture = value;
				}
			}
		}

		public ColorItem SelectedTheme
		{
			[DebuggerStepThrough]
			get
			{
				return _SelectedTheme;
			}
			set
			{
				if( _SelectedTheme == value )
				{
					return;
				}

				_SelectedTheme = value;
				RaisePropertyChanged();
			}
		}

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string _DatabasePath;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool _ExpandEntries;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _MoveDatabaseCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _OpenDatabaseFolderCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _ResetToDefaultsCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _SaveCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private ColorItem _SelectedAccent;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private CultureInfo _SelectedLanguage;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private ColorItem _SelectedTheme;

		private bool ChangeLanguage = true;
	}
}