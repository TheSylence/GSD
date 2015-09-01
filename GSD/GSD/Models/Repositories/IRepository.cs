using System.Collections.Generic;

namespace GSD.Models.Repositories
{
	internal interface IRepository<TEntity> where TEntity : Entity
	{
		void Add( TEntity entity );

		void Delete( TEntity entity );

		IEnumerable<TEntity> GetAll();

		void Update( TEntity entity );
	}
}