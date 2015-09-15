using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GSD.Messages;
using GSD.Models;
using GSD.Models.Repositories;
using GSD.Resources;
using GSD.ViewServices;

namespace GSD.ViewModels
{
	internal class TagListViewModel : ValidationViewModel, IResettable
	{
		public TagListViewModel( IProjectListViewModel projectList )
		{
			TagRepo = new TagRepository( Session );

			ProjectList = projectList;
			ProjectList.CurrentProjectChanged += ProjectList_CurrentProjectChanged;

			var tags = Enumerable.Empty<TagViewModel>();
			if( ProjectList.CurrentProject != null )
			{
				tags = ProjectList.CurrentProject.Model.Tags.Select( t => new TagViewModel( t ) );
			}

			Tags = new ObservableCollection<TagViewModel>( tags );

			AvailableColors = new List<Color>( GetAllColors().OrderBy( CalculateHue ) );
			TagNames = new List<string>();

			Validate( nameof( NewTagName ) ).Check( () => !string.IsNullOrWhiteSpace( NewTagName ) ).Message( Strings.TagMustHaveName );
			Validate( nameof( NewTagName ) ).Check( () => !TagNames.Contains( NewTagName ) ).Message( Strings.ThisNameIsAlreadyUsed );
			Reset();
		}

		public void Reset()
		{
			NewTagName = string.Empty;
			NewTagColor = AvailableColors.First();

			ReadTagNames();
			ClearValidationErrors();
		}

		private float CalculateHue( Color color )
		{
			var min = Math.Min( color.ScR, Math.Min( color.ScG, color.ScB ) );
			var max = Math.Max( color.ScR, Math.Max( color.ScG, color.ScB ) );
			float h;
			var delta = max - min;
			const float tolerance = 0.0001f;

			if( Math.Abs( color.ScR - max ) < tolerance )
			{
				h = ( color.ScG - color.ScB ) / delta;
			}
			else if( Math.Abs( color.ScG - max ) < tolerance )
			{
				h = 2 + ( color.ScB - color.ScR ) / delta;
			}
			else
			{
				h = 4 + ( color.ScR - color.ScG ) / delta;
			}

			h *= 60;

			if( h < 0 )
			{
				h += 360;
			}

			return h;
		}

		private bool CanExecuteDeleteTagCommand( TagViewModel arg )
		{
			return arg != null;
		}

		private bool CanExecuteNewTagCommand()
		{
			return !string.IsNullOrWhiteSpace( NewTagName ) && !TagNames.Contains( NewTagName );
		}

		private void ExecuteCloseFlyoutCommand()
		{
			MessengerInstance.Send( new FlyoutMessage( FlyoutMessage.TagFlyoutName ) );
		}

		private async void ExecuteDeleteTagCommand( TagViewModel arg )
		{
			ConfirmationServiceArgs args = new ConfirmationServiceArgs( Strings.Confirm,
				string.Format( Strings.DoYouReallyWantToDeleteTagXXX, arg.Model.Name ),
				Strings.Yes, Strings.No );

			if( !await ViewServices.Execute<IConfirmationService, bool>( args ) )
			{
				return;
			}

			TagRepo.Delete( arg.Model );
			Tags.Remove( arg );

			MessengerInstance.Send( new TagRemovedMessage( arg.Model ) );
			MessengerInstance.Send( new NotificationMessage( Strings.TagDeleted ) );
			ReadTagNames();
		}

		private void ExecuteNewTagCommand()
		{
			var tag = new Tag
			{
				Name = NewTagName,
				Project = ProjectList.CurrentProject.Model,
				Color = NewTagColor.ToString( CultureInfo.InvariantCulture ).Substring( 3 )
			};

			TagRepo.Add( tag );
			ProjectList.AddTag( tag );
			Tags.Add( new TagViewModel( tag ) );
			Reset();

			MessengerInstance.Send( new TagAddedMessage( tag ) );
		}

		private IEnumerable<Color> GetAllColors()
		{
			yield return Colors.AliceBlue;
			yield return Colors.AntiqueWhite;
			yield return Colors.Aqua;
			yield return Colors.Aquamarine;
			yield return Colors.Azure;
			yield return Colors.Beige;
			yield return Colors.Bisque;
			yield return Colors.Black;
			yield return Colors.BlanchedAlmond;
			yield return Colors.Blue;
			yield return Colors.BlueViolet;
			yield return Colors.Brown;
			yield return Colors.BurlyWood;
			yield return Colors.CadetBlue;
			yield return Colors.Chartreuse;
			yield return Colors.Chocolate;
			yield return Colors.Coral;
			yield return Colors.CornflowerBlue;
			yield return Colors.Cornsilk;
			yield return Colors.Crimson;
			yield return Colors.Cyan;
			yield return Colors.DarkBlue;
			yield return Colors.DarkCyan;
			yield return Colors.DarkGoldenrod;
			yield return Colors.DarkGray;
			yield return Colors.DarkGreen;
			yield return Colors.DarkKhaki;
			yield return Colors.DarkMagenta;
			yield return Colors.DarkOliveGreen;
			yield return Colors.DarkOrange;
			yield return Colors.DarkOrchid;
			yield return Colors.DarkRed;
			yield return Colors.DarkSalmon;
			yield return Colors.DarkSeaGreen;
			yield return Colors.DarkSlateBlue;
			yield return Colors.DarkSlateGray;
			yield return Colors.DarkTurquoise;
			yield return Colors.DarkViolet;
			yield return Colors.DeepPink;
			yield return Colors.DeepSkyBlue;
			yield return Colors.DimGray;
			yield return Colors.DodgerBlue;
			yield return Colors.Firebrick;
			yield return Colors.FloralWhite;
			yield return Colors.ForestGreen;
			yield return Colors.Fuchsia;
			yield return Colors.Gainsboro;
			yield return Colors.GhostWhite;
			yield return Colors.Gold;
			yield return Colors.Goldenrod;
			yield return Colors.Gray;
			yield return Colors.Green;
			yield return Colors.GreenYellow;
			yield return Colors.Honeydew;
			yield return Colors.HotPink;
			yield return Colors.IndianRed;
			yield return Colors.Indigo;
			yield return Colors.Ivory;
			yield return Colors.Khaki;
			yield return Colors.Lavender;
			yield return Colors.LavenderBlush;
			yield return Colors.LawnGreen;
			yield return Colors.LemonChiffon;
			yield return Colors.LightBlue;
			yield return Colors.LightCoral;
			yield return Colors.LightCyan;
			yield return Colors.LightGoldenrodYellow;
			yield return Colors.LightGray;
			yield return Colors.LightGreen;
			yield return Colors.LightPink;
			yield return Colors.LightSalmon;
			yield return Colors.LightSeaGreen;
			yield return Colors.LightSkyBlue;
			yield return Colors.LightSlateGray;
			yield return Colors.LightSteelBlue;
			yield return Colors.LightYellow;
			yield return Colors.Lime;
			yield return Colors.LimeGreen;
			yield return Colors.Linen;
			yield return Colors.Magenta;
			yield return Colors.Maroon;
			yield return Colors.MediumAquamarine;
			yield return Colors.MediumBlue;
			yield return Colors.MediumOrchid;
			yield return Colors.MediumPurple;
			yield return Colors.MediumSeaGreen;
			yield return Colors.MediumSlateBlue;
			yield return Colors.MediumSpringGreen;
			yield return Colors.MediumTurquoise;
			yield return Colors.MediumVioletRed;
			yield return Colors.MidnightBlue;
			yield return Colors.MintCream;
			yield return Colors.MistyRose;
			yield return Colors.Moccasin;
			yield return Colors.NavajoWhite;
			yield return Colors.Navy;
			yield return Colors.OldLace;
			yield return Colors.Olive;
			yield return Colors.OliveDrab;
			yield return Colors.Orange;
			yield return Colors.OrangeRed;
			yield return Colors.Orchid;
			yield return Colors.PaleGoldenrod;
			yield return Colors.PaleGreen;
			yield return Colors.PaleTurquoise;
			yield return Colors.PaleVioletRed;
			yield return Colors.PapayaWhip;
			yield return Colors.PeachPuff;
			yield return Colors.Peru;
			yield return Colors.Pink;
			yield return Colors.Plum;
			yield return Colors.PowderBlue;
			yield return Colors.Purple;
			yield return Colors.Red;
			yield return Colors.RosyBrown;
			yield return Colors.RoyalBlue;
			yield return Colors.SaddleBrown;
			yield return Colors.Salmon;
			yield return Colors.SandyBrown;
			yield return Colors.SeaGreen;
			yield return Colors.SeaShell;
			yield return Colors.Sienna;
			yield return Colors.Silver;
			yield return Colors.SkyBlue;
			yield return Colors.SlateBlue;
			yield return Colors.SlateGray;
			yield return Colors.Snow;
			yield return Colors.SpringGreen;
			yield return Colors.SteelBlue;
			yield return Colors.Tan;
			yield return Colors.Teal;
			yield return Colors.Thistle;
			yield return Colors.Tomato;
			yield return Colors.Transparent;
			yield return Colors.Turquoise;
			yield return Colors.Violet;
			yield return Colors.Wheat;
			yield return Colors.White;
			yield return Colors.WhiteSmoke;
			yield return Colors.Yellow;
			yield return Colors.YellowGreen;
		}

		private void ProjectList_CurrentProjectChanged( object sender, EventArgs e )
		{
			Tags.Clear();

			if( ProjectList.CurrentProject != null )
			{
				foreach( var tag in ProjectList.CurrentProject.Model.Tags )
				{
					Tags.Add( new TagViewModel( tag ) );
				}
			}
		}

		private void ReadTagNames()
		{
			TagNames = TagRepo.GetAllNames( ProjectList.CurrentProject?.Model ).ToList();
		}

		public List<Color> AvailableColors { get; }
		public ICommand CloseFlyoutCommand => _CloseFlyoutCommand ?? ( _CloseFlyoutCommand = new RelayCommand( ExecuteCloseFlyoutCommand ) );

		public RelayCommand<TagViewModel> DeleteTagCommand
			=>
				_DeleteTagCommand ??
				( _DeleteTagCommand = new RelayCommand<TagViewModel>( ExecuteDeleteTagCommand, CanExecuteDeleteTagCommand ) );

		public Color NewTagColor
		{
			[DebuggerStepThrough] get { return _NewTagColor; }
			set
			{
				if( _NewTagColor == value )
				{
					return;
				}

				_NewTagColor = value;
				RaisePropertyChanged();
			}
		}

		public ICommand NewTagCommand => _NewTagCommand ?? ( _NewTagCommand = new RelayCommand( ExecuteNewTagCommand, CanExecuteNewTagCommand ) );

		public string NewTagName
		{
			[DebuggerStepThrough] get { return _NewTagName; }
			set
			{
				if( _NewTagName == value )
				{
					return;
				}

				_NewTagName = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<TagViewModel> Tags { get; }
		private readonly IProjectListViewModel ProjectList;
		private readonly ITagRepository TagRepo;
		private RelayCommand _CloseFlyoutCommand;
		private RelayCommand<TagViewModel> _DeleteTagCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private Color _NewTagColor;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _NewTagCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string _NewTagName;

		private List<string> TagNames;
	}
}