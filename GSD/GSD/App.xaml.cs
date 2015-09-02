using GSD.Models;
using GSD.Models.Repositories;
using GSD.ViewServices;
using MahApps.Metro;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Linq;
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

			ConnectToDatabase();
			SetupDatabase();
			SetupViewServices();
			SetupStyle();
		}

		private static void ConnectToDatabase()
		{
			var cfg = new Configuration();
			cfg.AddAssembly( typeof( Project ).Assembly );
			cfg.Configure();

			new SchemaUpdate( cfg ).Execute( false, true );
			var factory = cfg.BuildSessionFactory();

			Session = factory.OpenSession();
		}

		private static void SetupViewServices()
		{
			ViewServices = new ViewServiceRepository();
			ViewServices.Register<IConfirmationService>( new ConfirmationService() );
		}

		private void SetupDatabase()
		{
			var repo = new SettingsRepository( Session );

			foreach( var kvp in SettingKeys.DefaultValues )
			{
				var cfg = repo.GetById( kvp.Key );
				if( cfg != null )
				{
					continue;
				}

				cfg = new Config { Id = kvp.Key, Value = kvp.Value };
				repo.Add( cfg );
			}
		}

		private void SetupStyle()
		{
			var repo = new SettingsRepository( Session );

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