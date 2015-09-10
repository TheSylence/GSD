using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GSD.Behaviors
{
	internal class CancelCommand : Behavior<TextBox>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.KeyDown += ( s, e ) =>
			{
				if( e.Key != Key.Escape || Command == null || !Command.CanExecute( CommandParameter ) )
				{
					return;
				}

				Command.Execute( CommandParameter );
			};
		}

		public ICommand Command
		{
			get { return (ICommand)GetValue( CommandProperty ); }
			set { SetValue( CommandProperty, value ); }
		}

		public object CommandParameter
		{
			get { return (object)GetValue( CommandParameterProperty ); }
			set { SetValue( CommandParameterProperty, value ); }
		}

		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register( "CommandParameter", typeof( object ), typeof( CancelCommand ), new PropertyMetadata( null ) );
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register( "Command", typeof( ICommand ), typeof( CancelCommand ), new PropertyMetadata( null ) );

	}
}