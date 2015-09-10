using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GSD.Behaviors
{
	internal class EnterCommand : Behavior<TextBox>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.KeyDown += ( s, e ) =>
			{
				if( e.Key != Key.Return || Command == null || !Command.CanExecute( CommandParameter ) )
				{
					return;
				}

				Command.Execute( CommandParameter );
				if( ClearAfterExecute )
				{
					AssociatedObject.Text = string.Empty;
				}
			};
		}

		public bool ClearAfterExecute
		{
			get { return (bool)GetValue( ClearAfterExecuteProperty ); }
			set { SetValue( ClearAfterExecuteProperty, value ); }
		}

		public ICommand Command
		{
			get { return (ICommand)GetValue( CommandProperty ); }
			set { SetValue( CommandProperty, value ); }
		}

		public object CommandParameter
		{
			get { return GetValue( CommandParameterProperty ); }
			set { SetValue( CommandParameterProperty, value ); }
		}

		public static readonly DependencyProperty ClearAfterExecuteProperty = DependencyProperty.Register( "ClearAfterExecute", typeof( bool ), typeof( EnterCommand ), new PropertyMetadata( true ) );
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register( "CommandParameter", typeof( object ), typeof( EnterCommand ), new PropertyMetadata( null ) );
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register( "Command", typeof( ICommand ), typeof( EnterCommand ), new PropertyMetadata( null ) );
	}
}