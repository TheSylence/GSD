using GSD.Models.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using GSD.ViewServices;

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

		/// <summary>
		///     Ruft die Validierungsfehler für eine angegebene Eigenschaft oder für die ganze Entität ab.
		/// </summary>
		/// <returns>
		///     Die Validierungsfehler für die Eigenschaft oder die Entität.
		/// </returns>
		/// <param name="propertyName">
		///     Der Name der Eigenschaft, von Validierungsfehlern abgerufen werden soll. null oder Fehlern
		///     abrufen, <see cref="F:System.String.Empty" />oder auf Entitätsebene.
		/// </param>
		public IEnumerable GetErrors( string propertyName )
		{
			if( !ValidationMap.ContainsKey( propertyName ) )
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
			}

			ErrorsChanged?.Invoke( this, new DataErrorsChangedEventArgs( null ) );
		}

		protected override void RaisePropertyChanged( [CallerMemberName] string propertyName = null )
		{
			Debug.Assert( propertyName != null, "propertyName != null" );

			List<ValidationBinder> binder;
			if( ValidationMap.TryGetValue( propertyName, out binder ) )
			{
				binder.ForEach( b => b.Update() );
				ErrorsChanged?.Invoke( this, new DataErrorsChangedEventArgs( propertyName ) );
			}
		}

		protected IValidationSetup Validate( string propertyName )
		{
			if( string.IsNullOrWhiteSpace( propertyName ) )
			{
				throw new ArgumentException( @"Invalid property name", nameof( propertyName ) );
			}

			return new ValidationSetup( this, propertyName );
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
			get
			{
				return ValidationMap.Values.Any( b => b.Any( bb => bb.HasError ) );
			}
		}

		private readonly Dictionary<string, List<ValidationBinder>> ValidationMap = new Dictionary<string, List<ValidationBinder>>();
	}
}