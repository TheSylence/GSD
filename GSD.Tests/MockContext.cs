using GSD.Models.Repositories;
using GSD.ViewServices;
using Moq;

namespace GSD.Tests
{
	internal class MockContext
	{
		public MockContext( bool strict = false )
		{
			MockBehavior behavior = strict ? MockBehavior.Strict : MockBehavior.Loose;

			SettingsRepoMock = new Mock<ISettingsRepository>( behavior );
			ProjectRepoMock = new Mock<IProjectRepository>( behavior );
			ViewServiceRepoMock = new Mock<IViewServiceRepository>( behavior );
		}

		public IProjectRepository ProjectRepo => ProjectRepoMock.Object;
		public Mock<IProjectRepository> ProjectRepoMock { get; }
		public ISettingsRepository SettingsRepo => SettingsRepoMock.Object;
		public Mock<ISettingsRepository> SettingsRepoMock { get; }
		public IViewServiceRepository ViewServiceRepo => ViewServiceRepoMock.Object;
		public Mock<IViewServiceRepository> ViewServiceRepoMock { get; }
	}
}