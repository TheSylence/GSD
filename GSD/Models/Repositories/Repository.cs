using System.Collections.Generic;
using NHibernate;

namespace GSD.Models.Repositories
{
	internal interface IRepository<TEntity> where TEntity : class
	{
		void Add( TEntity entity );

		void Delete( TEntity entity );

		IEnumerable<TEntity> GetAll();

		TEntity GetById( object id );

		void Update( TEntity entity );
	}

	internal abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		protected Repository( ISession session )
		{
			Session = session;
		}

		public void Add( TEntity entity )
		{
			using( var tx = Session.BeginTransaction() )
			{
				Session.Save( entity );
				tx.Commit();
			}
		}

		public void Delete( TEntity entity )
		{
			using( var tx = Session.BeginTransaction() )
			{
				Session.Delete( entity );
				tx.Commit();
			}
		}

		public IEnumerable<TEntity> GetAll()
		{
			return Session.CreateCriteria<TEntity>().List<TEntity>();
		}

		public TEntity GetById( object id )
		{
			return Session.Get<TEntity>( id );
		}

		public void Update( TEntity entity )
		{
			using( var tx = Session.BeginTransaction() )
			{
				Session.Update( entity );
				tx.Commit();
			}
		}

		protected ISession Session { get; }
	}
}