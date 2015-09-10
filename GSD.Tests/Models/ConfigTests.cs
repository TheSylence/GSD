using GSD.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GSD.Tests.Models
{
	[TestClass]
	public class ConfigTests
	{
		[TestMethod, TestCategory( "Models" )]
		public void GetReturnsConvertedValue()
		{
			// Arrange
			var cfg = new Config
			{
				Value = "True"
			};

			// Act
			bool b = cfg.Get<bool>();

			cfg.Value = "123";
			int i = cfg.Get<int>();

			// Assert
			Assert.AreEqual( true, b );
			Assert.AreEqual( 123, i );
		}
	}
}