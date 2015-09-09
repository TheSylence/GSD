using System;

namespace GSD.ViewModels
{
	internal interface IValidationSetup
	{
		IValidationSetup Check( Func<bool> action );

		void Message( string message );
	}

	internal class ValidationSetup : IValidationSetup
	{
		public ValidationSetup( ValidationViewModel vm, string propertyName )
		{
			ViewModel = vm;
			PropertyName = propertyName;
		}

		IValidationSetup IValidationSetup.Check( Func<bool> action )
		{
			Check = action;
			return this;
		}

		void IValidationSetup.Message( string message )
		{
			if( Check == null )
			{
				throw new InvalidOperationException( "No check constraint set" );
			}

			ViewModel.AddValidation( PropertyName, new ValidationBinder( Check, message ) );
		}

		private readonly string PropertyName;
		private readonly ValidationViewModel ViewModel;

		private Func<bool> Check;
	}
}