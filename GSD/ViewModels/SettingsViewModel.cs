using GalaSoft.MvvmLight.CommandWpf;
using GSD.Models.Repositories;
using MahApps.Metro;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace GSD.ViewModels
{
	internal class SettingsViewModel : ViewModelBaseEx, IResettable
	{
		public SettingsViewModel()
		{
			AvailableAccents = ThemeManager.Accents.Select( a => new ColorItem
			{
				Name = a.Name,
				BorderBrush = a.Resources["AccentColorBrush"] as Brush
			} ).ToList();

			AvailableThemes = ThemeManager.AppThemes.Select( t => new ColorItem
			{
				Name = t.Name,
				ColorBrush = t.Resources["WhiteColorBrush"] as Brush,
				BorderBrush = t.Resources["BlackColorBrush"] as Brush
			} ).ToList();
		}

		public void Reset()
		{
			var accent = Settings.GetById( SettingKeys.Accent ).Value;
			var theme = Settings.GetById( SettingKeys.Theme ).Value;

			SelectedAccent = AvailableAccents.FirstOrDefault( a => a.Name == accent );
			SelectedTheme = AvailableThemes.FirstOrDefault( t => t.Name == theme );

			ExpandEntries = Settings.GetById( SettingKeys.ExpandEntries ).Get<bool>();
		}

		private void ExecuteResetToDefaultsCommand()
		{
			foreach( var key in new[] { SettingKeys.Accent, SettingKeys.Theme } )
			{
				Settings.Set( key, SettingKeys.DefaultValues[key] );
			}
		}

		private void ExecuteSaveCommand()
		{
			Settings.Set( SettingKeys.Accent, SelectedAccent.Name );
			Settings.Set( SettingKeys.Theme, SelectedTheme.Name );
			Settings.Set( SettingKeys.ExpandEntries, ExpandEntries.ToString() );

			var accent = ThemeManager.Accents.FirstOrDefault( a => a.Name == SelectedAccent.Name );
			var theme = ThemeManager.AppThemes.FirstOrDefault( t => t.Name == SelectedTheme.Name );

			ThemeManager.ChangeAppStyle( Application.Current, accent, theme );
		}

		public List<ColorItem> AvailableAccents { get; }

		public List<ColorItem> AvailableThemes { get; }

		public bool ExpandEntries
		{
			[DebuggerStepThrough]
			get
			{
				return _ExpandEntries;
			}
			set
			{
				if( _ExpandEntries == value )
				{
					return;
				}

				_ExpandEntries = value;
				RaisePropertyChanged();
			}
		}

		public RelayCommand ResetToDefaultsCommand => _ResetToDefaultsCommand ?? ( _ResetToDefaultsCommand = new RelayCommand( ExecuteResetToDefaultsCommand ) );

		public RelayCommand SaveCommand => _SaveCommand ?? ( _SaveCommand = new RelayCommand( ExecuteSaveCommand ) );

		public ColorItem SelectedAccent
		{
			[DebuggerStepThrough]
			get
			{
				return _SelectedAccent;
			}
			set
			{
				if( _SelectedAccent == value )
				{
					return;
				}

				_SelectedAccent = value;
				RaisePropertyChanged();
			}
		}

		public ColorItem SelectedTheme
		{
			[DebuggerStepThrough]
			get
			{
				return _SelectedTheme;
			}
			set
			{
				if( _SelectedTheme == value )
				{
					return;
				}

				_SelectedTheme = value;
				RaisePropertyChanged();
			}
		}

		[System.Diagnostics.DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool _ExpandEntries;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _ResetToDefaultsCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _SaveCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private ColorItem _SelectedAccent;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private ColorItem _SelectedTheme;
	}
}