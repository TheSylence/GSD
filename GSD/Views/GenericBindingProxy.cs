using GSD.ViewModels;
using System.Windows;

namespace GSD.Views
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	internal class GenericBindingProxy<T> : Freezable
	{
		protected override Freezable CreateInstanceCore()
		{
			return new GenericBindingProxy<T>();
		}

		public T Data
		{
			get { return (T)GetValue( DataProperty ); }
			set { SetValue( DataProperty, value ); }
		}

		public static readonly DependencyProperty DataProperty = DependencyProperty.Register( "Data", typeof( T ), typeof( GenericBindingProxy<T> ),
			new UIPropertyMetadata( null ) );
	}

	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	internal class MainBindingProxy : GenericBindingProxy<MainViewModel>
	{
	}

	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	internal class ProjectListBindingProxy : GenericBindingProxy<ProjectListViewModel>
	{
	}

	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	internal class TagListBindingProxy : GenericBindingProxy<TagListViewModel>
	{
	}
}