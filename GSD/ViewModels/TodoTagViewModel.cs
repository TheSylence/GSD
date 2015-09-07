﻿using System;
using System.Diagnostics;
using GSD.Models;

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

		public bool RaiseSelectionEvents { get; set; } = true;

		public bool IsSelected
		{
			[DebuggerStepThrough]
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

				if( RaiseSelectionEvents )
				{
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
		}

		public Tag Model { get; }

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool _IsSelected;

		private Todo Todo;
	}
}