using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace GSD.ViewModels.Utilities
{
	internal interface ITaskRunner
	{
		Task<T> Run<T>( Func<T> action );

		Task Run( Action action );
	}

	[ExcludeFromCodeCoverage]
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