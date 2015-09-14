using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;

namespace GSD.ViewModels.Utilities
{
	[ExcludeFromCodeCoverage]
	internal class ColorItem
	{
		public Brush BorderBrush { get; set; }
		public Brush ColorBrush { get; set; }
		public string Name { get; set; }
	}
}