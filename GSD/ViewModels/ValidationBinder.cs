using System;

namespace GSD.ViewModels
{
	internal class ValidationBinder
	{
		public ValidationBinder( Func<bool> check, string erorrMessage )
		{
			Check = check;
			ErrorMessage = erorrMessage;
		}

		internal void Update()
		{
			Error = null;
			HasError = false;

			try
			{
				if( !Check() )
				{
					Error = ErrorMessage;
					HasError = true;
				}
			}
			catch( Exception ex )
			{
				HasError = true;
				Error = ex.Message;
			}
		}

		internal string Error { get; private set; }
		internal bool HasError { get; private set; }

		private readonly Func<bool> Check;
		private readonly string ErrorMessage;

		public void Clear()
		{
			Error = null;
			HasError = false;
		}
	}
}