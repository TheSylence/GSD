using System.Windows;
using GSD.Models;
using GSD.Models.Repositories;
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

			//using( var session = SessionFactory.OpenSession() )
			//using( var tx = session.BeginTransaction() )
			//{
			//	var projectRepository = new ProjectRepository( session );
			//	var todoRepository = new TodoRepository( session );
			//	var tagRepository = new TagRepository( session );

			//	var project = new Project { Name = "Testproject" };
			//	projectRepository.Add( project );

			//	var bugTag = new Tag { Name = "Bug", Project = project };
			//	var taskTag = new Tag { Name = "Task", Project = project };

			//	tagRepository.Add( bugTag );
			//	tagRepository.Add( taskTag );

			//	project.Tags.Add( bugTag );
			//	project.Tags.Add( taskTag );
			//	projectRepository.Update( project );

			//	var todo = new Todo { Summary = "TestBug", Project = project, Details = "This is a test" };
			//	todoRepository.Add( todo );

			//	todo.Tags.Add( bugTag );
			//	todoRepository.Update( todo );

			//	project.Todos.Add( todo );

			//	projectRepository.Update( project );

			//	tx.Commit();
			//}
		}

		public static ISessionFactory SessionFactory { get; private set; }
	}
}