using GSD.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GSD.Tests.Models
{
	[TestClass]
	public class TodoTests
	{
		[TestMethod, TestCategory( "Models" )]
		public void NewTodoIsNotDone()
		{
			// Arrange

			// Act
			var todo = new Todo();

			// Assert
			Assert.IsFalse( todo.Done );
		}
	}
}