using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GSD.Models.Repositories;
using GSD.ViewServices;
using MahApps.Metro;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;

namespace GSD
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		protected override void OnExit( ExitEventArgs e )
		{
			Session.Dispose();

			base.OnExit( e );
		}

		protected override void OnStartup( StartupEventArgs e )
		{
			base.OnStartup( e );

#if DEBUG
			LocalizeDictionary.Instance.MissingKeyEvent += ( s, args ) =>
			{
				Debug.WriteLine( $"Error: Resource key not found: '{args.Key}'" );
			};
#endif

			SetupViewServices();
			ApplySettings();
		}

		public static void ConnectToDatabase()
		{
			var repo = new SettingsRepository();
			string fileName = repo.GetById( SettingKeys.DatabasePath )?.Value;
			if( string.IsNullOrWhiteSpace( fileName ) )
			{
				fileName = Constants.DefaultDatabasePath;
			}

			var dbConfig = new SQLiteConfiguration().UsingFile( fileName )

#if !DEBUG
				.DoNot
#endif
				.ShowSql();

			var cfg = Fluently.Configure().Database( dbConfig )
				.Mappings( map => map.FluentMappings.AddFromAssembly( Assembly.GetExecutingAssembly() ) )
				.BuildConfiguration();

			new SchemaUpdate( cfg ).Execute( false, true );
			var factory = cfg.BuildSessionFactory();

			Session = factory.OpenSession();
		}

		private static void SetupViewServices()
		{
			ViewServices = new ViewServiceRepository();
			ViewServices.Register<IConfirmationService>( new ConfirmationService() );
			ViewServices.Register<IInfoService>( new InfoService() );
			ViewServices.Register<IMoveDatabaseService>( new MoveDatabaseService() );
			ViewServices.Register<IBrowseFolderService>( new BrowseFolderService() );
		}

		private void ApplySettings()
		{
			var repo = new SettingsRepository();

			var themeName = repo.GetById( SettingKeys.Theme ).Value;
			var accentName = repo.GetById( SettingKeys.Accent ).Value;

			var accent = ThemeManager.Accents.FirstOrDefault( a => a.Name.Equals( accentName ) );
			var theme = ThemeManager.AppThemes.FirstOrDefault( t => t.Name.Equals( themeName ) );

			ThemeManager.ChangeAppStyle( this, accent, theme );

			ResxLocalizationProvider.Instance.UpdateCultureList( GetType().Assembly.FullName, "Strings" );
			var availableCultures = ResxLocalizationProvider.Instance.AvailableCultures.ToList();

			var lang = repo.GetById( SettingKeys.Language ).Value;
			var cultureInfo = CultureInfo.CreateSpecificCulture( lang );

			while( !Equals( cultureInfo, CultureInfo.InvariantCulture ) && !availableCultures.Contains( cultureInfo ) )
			{
				cultureInfo = cultureInfo.Parent;
			}

			if( Equals( cultureInfo, CultureInfo.InvariantCulture ) )
			{
				cultureInfo = new CultureInfo( "en" );
			}

			LocalizeDictionary.Instance.Culture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
			Thread.CurrentThread.CurrentCulture = cultureInfo;
		}

		public static ISession Session { get; private set; }
		public static IViewServiceRepository ViewServices { get; private set; }
	}
}