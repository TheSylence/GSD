using System.Threading.Tasks;

namespace GSD.ViewServices
{
	public interface IViewServiceRepository
	{
		Task<object> Execute<TService>( object args = null ) where TService : IViewService;
		Task<TResult> Execute<TService, TResult>( object args ) where TService : IViewService;
		void Register<TService>( IViewService service ) where TService : IViewService;
	}
}