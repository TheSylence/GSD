using System.Diagnostics;
using GalaSoft.MvvmLight;

namespace GSD.ViewModels
{
	internal class TagEntry : ObservableObject
	{
		public TagEntry( TagViewModel t )
		{
			Tag = t;
		}

		public bool IsSelected
		{
			[DebuggerStepThrough] get { return _IsSelected; }
			set
			{
				if( _IsSelected == value )
				{
					return;
				}

				_IsSelected = value;
				RaisePropertyChanged();
			}
		}

		public TagViewModel Tag { get; }

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool _IsSelected;
	}
}