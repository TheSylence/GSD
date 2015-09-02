using System.Windows;
using GSD.Models;
using GSD.ViewServices;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace GSD
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup( StartupEventArgs e )
		{
			base.OnStartup( e );

			var cfg = new Configuration();
			cfg.AddAssembly( typeof( Project ).Assembly );
			cfg.Configure();

			new SchemaUpdate( cfg ).Execute( false, true );
			SessionFactory = cfg.BuildSessionFactory();

			ViewServices = new ViewServiceRepository();
			ViewServices.Register<IConfirmationService>( new ConfirmationService() );
		}

		public static ISessionFactory SessionFactory { get; private set; }
		public static IViewServiceRepository ViewServices { get; private set; }
	}
}