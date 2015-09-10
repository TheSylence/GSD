using GSD.Models.Repositories;
using GSD.ViewServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GSD.ViewModels
{
	internal abstract class ValidationViewModel : ViewModelBaseEx, INotifyDataErrorInfo
	{
		protected ValidationViewModel( IViewServiceRepository viewServices = null, ISettingsRepository settingsRepo = null )
			: base( viewServices, settingsRepo )
		{
		}

		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		public void ClearValidationRules()
		{
			ValidationMap.Clear();
		}

		public IEnumerable GetErrors( string propertyName )
		{
			if( string.IsNullOrEmpty( propertyName ) || !ValidationMap.ContainsKey( propertyName ) )
			{
				return Enumerable.Empty<string>();
			}

			Debug.WriteLine( $"IDataError[{propertyName}]" );

			return ValidationMap[propertyName].Where( b => b.HasError ).Select( b => b.Error );
		}

		internal void AddValidation( string propertyName, ValidationBinder binder )
		{
			List<ValidationBinder> bindList;
			if( !ValidationMap.TryGetValue( propertyName, out bindList ) )
			{
				bindList = new List<ValidationBinder>();
				ValidationMap.Add( propertyName, bindList );
			}

			bindList.Add( binder );
		}

		protected void ClearValidationErrors()
		{
			foreach( var kvp in ValidationMap )
			{
				foreach( var binder in kvp.Value )
				{
					binder.Clear();
				}

				RaiseErrorsChanged( kvp.Key );
			}

			RaiseErrorsChanged( null );
		}

		protected void RaiseErrorsChanged( string propertyName )
		{
			ErrorsChanged?.Invoke( this, new DataErrorsChangedEventArgs( propertyName ) );
		}

		protected override void RaisePropertyChanged( [CallerMemberName] string propertyName = null )
		{
			ValidateProperty( propertyName );
		}

		protected IValidationSetup Validate( string propertyName )
		{
			if( string.IsNullOrWhiteSpace( propertyName ) )
			{
				throw new ArgumentException( @"Invalid property name", nameof( propertyName ) );
			}

			return new ValidationSetup( this, propertyName );
		}

		protected void ValidateProperty( string propertyName )
		{
			Debug.Assert( propertyName != null, "propertyName != null" );

			List<ValidationBinder> binder;
			if( ValidationMap.TryGetValue( propertyName, out binder ) )
			{
				binder.ForEach( b => b.Update() );
				RaiseErrorsChanged( propertyName );
			}
		}

		public string Error
		{
			get
			{
				var lines = ValidationMap.Values.Where( b => b.Any( bb => bb.HasError ) ).SelectMany( b => b ).Select( b => b.Error ).Where( x => x != null ).ToArray();
				return lines.Any() ? string.Join( Environment.NewLine, lines ) : null;
			}
		}

		public bool HasErrors
		{
			get { return ValidationMap.Values.Any( b => b.Any( bb => bb.HasError ) ); }
		}

		private readonly Dictionary<string, List<ValidationBinder>> ValidationMap = new Dictionary<string, List<ValidationBinder>>();
	}
}