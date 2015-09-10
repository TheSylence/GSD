using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GSD.Models.Repositories;
using GSD.ViewServices;
using MahApps.Metro;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace GSD
{
	/// <summary>
	/// Interaction logic for App.xaml
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

			SetupViewServices();
			SetupStyle();
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
		}

		private void SetupStyle()
		{
			var repo = new SettingsRepository();

			var themeName = repo.GetById( SettingKeys.Theme ).Value;
			var accentName = repo.GetById( SettingKeys.Accent ).Value;

			var accent = ThemeManager.Accents.FirstOrDefault( a => a.Name.Equals( accentName ) );
			var theme = ThemeManager.AppThemes.FirstOrDefault( t => t.Name.Equals( themeName ) );

			ThemeManager.ChangeAppStyle( this, accent, theme );
		}

		public static ISession Session { get; private set; }
		public static IViewServiceRepository ViewServices { get; private set; }
	}
}