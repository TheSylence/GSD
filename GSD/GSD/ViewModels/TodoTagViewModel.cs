using GSD.Models;
using System;

namespace GSD.ViewModels
{
	internal class TodoTagViewModel : ViewModelBaseEx
	{
		public TodoTagViewModel( Todo todo, Tag tag )
		{
			Todo = todo;
			Model = tag;
		}

		public event EventHandler Deselected;

		public event EventHandler Selected;

		public bool IsSelected
		{
			[System.Diagnostics.DebuggerStepThrough]
			get
			{
				return _IsSelected;
			}
			set
			{
				if( _IsSelected == value )
				{
					return;
				}

				_IsSelected = value;
				RaisePropertyChanged();

				if( value )
				{
					Selected?.Invoke( this, EventArgs.Empty );
				}
				else
				{
					Deselected?.Invoke( this, EventArgs.Empty );
				}
			}
		}

		public Tag Model { get; }

		[System.Diagnostics.DebuggerBrowsable( System.Diagnostics.DebuggerBrowsableState.Never )]
		private bool _IsSelected;

		private Todo Todo;
	}
}