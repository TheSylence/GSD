using GSD.Models;
using GSD.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GSD.Tests.ViewModels
{
	[TestClass]
	public class EditEntryViewModelTests
	{
		[TestMethod, TestCategory( "ViewModels" )]
		public void ConstructorExtractsCorrectModelData()
		{
			// Arrange
			var todo = new Todo {Summary = "summary", Details = "details"};
			var entry = new TodoViewModel( todo );

			// Act
			var vm = new EditEntryViewModel( entry );

			// Assert
			Assert.AreEqual( "summary", vm.Summary );
			Assert.AreEqual( "details", vm.Details );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void SaveCommandNeedsSummary()
		{
			// Arrange
			var todo = new Todo {Summary = "summary", Details = "details"};
			var entry = new TodoViewModel( todo );
			var vm = new EditEntryViewModel( entry );

			// Act
			bool withSummary = vm.SaveCommand.CanExecute( null );
			vm.Summary = string.Empty;
			bool withoutSummary = vm.SaveCommand.CanExecute( null );

			// Assert
			Assert.IsTrue( withSummary );
			Assert.IsFalse( withoutSummary );
		}
	}
}