using System;
using System.Threading.Tasks;

namespace GSD.ViewModels
{
	internal interface ITaskRunner
	{
		Task<T> Run<T>( Func<T> action );

		Task Run( Action action );
	}

	internal class TaskRunner : ITaskRunner
	{
		public Task<T> Run<T>( Func<T> action )
		{
			return Task.Run<T>( action );
		}

		public Task Run( Action action )
		{
			return Task.Run( action );
		}
	}
}