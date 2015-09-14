using GalaSoft.MvvmLight.CommandWpf;
using GSD.Models.Repositories;
using GSD.ViewServices;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;

namespace GSD.ViewModels
{
	internal class SettingsViewModel : ViewModelBaseEx, IResettable
	{
		public SettingsViewModel()
			: this( null )
		{
		}

		public SettingsViewModel( IViewServiceRepository viewServices, ISettingsRepository settingsRepo = null,
			IAppThemes themes = null, IStartupConfigurator startup = null )
			: base( viewServices, settingsRepo )
		{
			Themes = themes ?? new AppThemes();
			Startup = startup ?? new StartupConfigurator();

			AvailableAccents = Themes.Accents.ToList();
			AvailableThemes = Themes.Themes.ToList();

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
			StartMinimized = Settings.GetById( SettingKeys.StartMinimized ).Get<bool>();
			StartWithWindows = Settings.GetById( SettingKeys.StartWithWindows ).Get<bool>();
			CloseToTray = Settings.GetById( SettingKeys.CloseToTray ).Get<bool>();

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

			await ViewServices.Execute<IProgressService>( new ProgressServiceArgs( report =>
			{
				SQLiteBackupCallback callback =
					( source, sourceName, destination, destinationName, pages, remainingPages, totalPages, retry ) =>
					{
						report.SetProgress( totalPages - remainingPages, totalPages );
						return true;
					};

				SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder
				{
					DataSource = newPath
				};

				using( SQLiteConnection destConnection = new SQLiteConnection( sb.ToString() ) )
				{
					destConnection.Open();

					var connection = App.Session.Connection as SQLiteConnection;
					Debug.Assert( connection != null, "connection != null" );
					connection.BackupDatabase( destConnection, "main", "main", -1, callback, 100 );
				}
			} ) );

			Settings.Set( SettingKeys.DatabasePath, newPath );
			Application.Current.Shutdown();
			Process.Start( Assembly.GetExecutingAssembly().Location );
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

			Reset();
		}

		private void ExecuteSaveCommand()
		{
			Settings.Set( SettingKeys.Accent, SelectedAccent.Name );
			Settings.Set( SettingKeys.Theme, SelectedTheme.Name );
			Settings.Set( SettingKeys.ExpandEntries, ExpandEntries.ToString() );
			Settings.Set( SettingKeys.Language, SelectedLanguage.IetfLanguageTag );
			Settings.Set( SettingKeys.StartWithWindows, StartWithWindows.ToString() );
			Settings.Set( SettingKeys.StartMinimized, StartMinimized.ToString() );
			Settings.Set( SettingKeys.CloseToTray, CloseToTray.ToString() );

			Themes.ChangeStyle( SelectedTheme.Name, SelectedAccent.Name );

			Startup.SetStartup( StartWithWindows, Constants.AutostartArgument );
		}

		public List<ColorItem> AvailableAccents { get; }

		public List<CultureInfo> AvailableLanguages { get; }

		public List<ColorItem> AvailableThemes { get; }

		public bool CloseToTray
		{
			[DebuggerStepThrough]
			get
			{
				return _CloseToTray;
			}
			set
			{
				if( _CloseToTray == value )
				{
					return;
				}

				_CloseToTray = value;
				RaisePropertyChanged( nameof( CloseToTray ) );
			}
		}

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

				if( !ChangeLanguage )
				{
					return;
				}

				LocalizeDictionary.Instance.Culture = value;
				Thread.CurrentThread.CurrentUICulture = value;
				Thread.CurrentThread.CurrentCulture = value;
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

		public bool StartMinimized
		{
			[DebuggerStepThrough]
			get
			{
				return _StartMinimized;
			}
			set
			{
				if( _StartMinimized == value )
				{
					return;
				}

				_StartMinimized = value;
				RaisePropertyChanged();
			}
		}

		public bool StartWithWindows
		{
			[DebuggerStepThrough]
			get
			{
				return _StartWithWindows;
			}
			set
			{
				if( _StartWithWindows == value )
				{
					return;
				}

				_StartWithWindows = value;
				RaisePropertyChanged();
			}
		}

		private readonly IStartupConfigurator Startup;

		private readonly IAppThemes Themes;

		[System.Diagnostics.DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool _CloseToTray;

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

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool _StartMinimized;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool _StartWithWindows;

		private bool ChangeLanguage = true;
	}
}