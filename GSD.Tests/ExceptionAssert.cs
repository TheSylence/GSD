using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GSD.Tests
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	internal static class ExceptionAssert
	{
		public static void Throws<TException, TResult>( Func<Task<TResult>> action ) where TException : Exception
		{
			Task<TResult> task = action();
			Exception catched = null;

			try
			{
				task.Wait();
			}
			catch( AggregateException ex )
			{
				catched = ex.InnerExceptions.FirstOrDefault();
			}
			catch( Exception ex )
			{
				catched = ex;
			}

			CheckException<TException>( catched );
		}

		public static void Throws<TException>( Action action ) where TException : Exception
		{
			Exception catched = null;

			try
			{
				action();
			}
			catch( Exception ex )
			{
				catched = ex;
			}

			CheckException<TException>( catched );
		}

		private static void CheckException<TException>( Exception catched ) where TException : Exception
		{
			if( catched == null )
			{
				Assert.Fail( $"No exception was thrown although exception of type {typeof( TException )} was expected" );
			}

			if( catched.GetType() != typeof( TException ) )
			{
				Assert.Fail( $"Exception '{catched.Message}' was thrown instead of {typeof( TException )}" );
			}
		}
	}
}