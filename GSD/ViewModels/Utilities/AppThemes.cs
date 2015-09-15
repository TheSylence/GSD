using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro;

namespace GSD.ViewModels.Utilities
{
	internal interface IAppThemes
	{
		IEnumerable<ColorItem> Accents { get; }
		IEnumerable<ColorItem> Themes { get; }
		void ChangeStyle( string theme, string accent );
	}

	[ExcludeFromCodeCoverage]
	internal class AppThemes : IAppThemes
	{
		public IEnumerable<ColorItem> Accents => ThemeManager.Accents.Select( a => new ColorItem
		{
			Name = a.Name,
			ColorBrush = a.Resources["AccentColorBrush"] as Brush
		} );

		public IEnumerable<ColorItem> Themes => ThemeManager.AppThemes.Select( t => new ColorItem
		{
			Name = t.Name,
			ColorBrush = t.Resources["WhiteColorBrush"] as Brush,
			BorderBrush = t.Resources["BlackColorBrush"] as Brush
		} );

		public void ChangeStyle( string themeName, string accentName )
		{
			var accent = ThemeManager.Accents.FirstOrDefault( a => a.Name == accentName );
			var theme = ThemeManager.AppThemes.FirstOrDefault( t => t.Name == themeName );

			ThemeManager.ChangeAppStyle( Application.Current, accent, theme );
		}
	}
}