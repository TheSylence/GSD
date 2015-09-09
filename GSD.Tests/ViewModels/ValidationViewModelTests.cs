using GSD.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Util;
using System;
using System.Diagnostics;

namespace GSD.Tests.ViewModels
{
	[TestClass]
	public class ValidationViewModelTests
	{
		[TestMethod, TestCategory( "ViewModels" )]
		public void ErrorIsCorrectlyConstructed()
		{
			// Arrange
			var vm = new TestValidation();

			vm.ValidateWrapper( nameof( vm.Name ) ).Check( () => vm.Name != "test" ).Message( "firstCheck" );
			vm.ValidateWrapper( nameof( vm.Name ) ).Check( () => !string.IsNullOrEmpty( vm.Name ) ).Message( "secondCheck" );

			// Act
			vm.Name = "test";
			vm.Name = null;
			string before = vm.Error;

			vm.Name = "test";
			string after = vm.Error;

			vm.Name = "123";
			string last = vm.Error;

			// Assert
			Assert.AreEqual( "secondCheck", before );
			Assert.AreEqual( "firstCheck", after );
			Assert.IsNull( last, last );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ErrorWithMultipleLinesIsCorrectlyConstructed()
		{
			// Arrange
			var vm = new TestValidation();

			vm.ValidateWrapper( nameof( vm.Name ) ).Check( () => !string.IsNullOrEmpty( vm.Name ) ).Message( "firstCheck" );
			vm.ValidateWrapper( nameof( vm.Name ) ).Check( () => !string.IsNullOrEmpty( vm.Name ) ).Message( "secondCheck" );

			// Act
			vm.Name = "test";
			vm.Name = null;
			string error = vm.Error;

			// Assert
			Assert.AreEqual( "firstCheck" + Environment.NewLine + "secondCheck", error );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ExceptionInValidationActionIsSwallowed()
		{
			// Arrange
			var vm = new TestValidation();

			// Act
			vm.ValidateWrapper( nameof( vm.Name ) ).Check( () => { throw new Exception( "test error" ); } ).Message( "test message" );

			vm.Name = "test";

			// Assert
			Assert.AreEqual( "test error", vm.GetErrors( nameof( vm.Name ) ).First() );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void HasErrorIsCorrectlySet()
		{
			// Arrange
			var vm = new TestValidation();

			vm.ValidateWrapper( nameof( vm.Name ) ).Check( () => vm.Name != "test" ).Message( "firstCheck" );
			vm.ValidateWrapper( nameof( vm.Name ) ).Check( () => !string.IsNullOrEmpty( vm.Name ) ).Message( "secondCheck" );

			// Act
			vm.Name = "test";
			vm.Name = null;
			bool before = vm.HasErrors;

			vm.Name = "test";
			bool after = vm.HasErrors;

			vm.Name = "123";
			bool last = vm.HasErrors;

			// Assert
			Assert.IsTrue( before );
			Assert.IsTrue( after );
			Assert.IsFalse( last );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void PropertyWithoutValidationIsNotValidated()
		{
			// Arrange
			var vm = new TestValidation
			{
				// Act
				Name = "test"
			};

			// Assert
			Assert.IsFalse( vm.GetErrors( nameof( vm.Name ) ).Any() );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void UnchangedPropertyIsNotValidated()
		{
			// Arrange
			var vm = new TestValidation();

			// Act
			vm.ValidateWrapper( nameof( vm.Name ) ).Check( () => { throw new Exception( "must not be called" ); } ).Message( "must not be visible" );

			// Assert
			var errors = vm.GetErrors( nameof( vm.Name ) );

			Assert.IsFalse( errors.Any() );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ValidateThrowsArgumentExceptionWithEmptyPropertyName()
		{
			// Arrange
			var vm = new TestValidation();

			// Act
			Action action = () => vm.ValidateWrapper( " " );

			// Assert
			ExceptionAssert.Throws<ArgumentException>( action );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ValidationMessageIsCorrectlySet()
		{
			// Arrange
			var vm = new TestValidation();

			// Act
			vm.ValidateWrapper( nameof( vm.Name ) ).Check( () => false ).Message( "test message" );
			vm.Name = "test";

			// Assert
			Assert.AreEqual( "test message", vm.GetErrors( nameof( vm.Name ) ).First() );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ValidationsCanBeRemoved()
		{
			// Arrange
			var vm = new TestValidation();

			vm.ValidateWrapper( nameof( vm.Name ) ).Check( () => vm.Name != "test" ).Message( "test" );

			// Act
			vm.Name = "test";
			string before = (string)vm.GetErrors( nameof( vm.Name ) ).First();
			vm.ClearValidationRules();
			vm.Name = "123";
			vm.Name = "test";
			string after = (string)vm.GetErrors( nameof( vm.Name ) ).FirstOrNull();

			// Assert
			Assert.AreEqual( "test", before );
			Assert.IsNull( after );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ValidationWithoutCheckThrowsException()
		{
			// Arrange
			var vm = new TestValidation();

			// Act
			Action action = () => vm.ValidateWrapper( nameof( vm.Name ) ).Message( "test" );

			// Assert
			ExceptionAssert.Throws<InvalidOperationException>( action );
		}

		[TestMethod, TestCategory( "ViewModels" )]
		public void ValidPropertyValueIsValidated()
		{
			// Arrange
			var vm = new TestValidation();

			// Act
			vm.ValidateWrapper( nameof( vm.Name ) ).Check( () => true ).Message( "must not be visible" );
			vm.Name = "test";

			// Assert
			Assert.IsFalse( vm.GetErrors( nameof( vm.Name ) ).Any() );
		}

		private class TestValidation : ValidationViewModel
		{
			public IValidationSetup ValidateWrapper( string propertyName )
			{
				return Validate( propertyName );
			}

			public string Name
			{
				[DebuggerStepThrough] get { return _Name; }
				set
				{
					if( _Name == value )
					{
						return;
					}

					_Name = value;
					RaisePropertyChanged( nameof( Name ) );
				}
			}

			[DebuggerBrowsable( DebuggerBrowsableState.Never )] private string _Name;
		}
	}
}