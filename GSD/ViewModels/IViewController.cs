using System;

namespace GSD.ViewModels
{
	public interface IViewController
	{
		event EventHandler CloseRequested;
	}
}