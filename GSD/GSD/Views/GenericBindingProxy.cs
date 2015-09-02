using System.Windows;
using GSD.ViewModels;

namespace GSD.Views
{
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

		public static readonly DependencyProperty DataProperty = DependencyProperty.Register( "Data", typeof( T ), typeof( GenericBindingProxy<T> ), new UIPropertyMetadata( null ) );
	}

	internal class ProjectListBindingProxy : GenericBindingProxy<ProjectListViewModel>
	{ }
}