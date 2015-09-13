using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace GSD.ViewServices
{
	public interface IViewService
	{
		Task<object> Execute( MetroWindow window, object args );
	}

	internal class ViewServiceRepository : IViewServiceRepository
	{
		public async Task<object> Execute<TService>( object args = null ) where TService : IViewService
		{
			Type type = typeof( TService );

			IViewService service;
			if( !ServiceMap.TryGetValue( type, out service ) )
			{
				throw new InvalidOperationException( $"Unknown service of type {type}" );
			}

			return await service.Execute( Window, args );
		}

		public async Task<TResult> Execute<TService, TResult>( object args = null ) where TService : IViewService
		{
			object result = await Execute<TService>( args );
            return (TResult)( result );
		}

		public void Register<TService>( IViewService service ) where TService : IViewService
		{
			ServiceMap.Add( typeof( TService ), service );
		}

		private static MetroWindow Window => Application.Current?.MainWindow as MetroWindow;

		private readonly Dictionary<Type, IViewService> ServiceMap = new Dictionary<Type, IViewService>();
	}
}