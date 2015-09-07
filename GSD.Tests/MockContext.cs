using GSD.Models.Repositories;
using Moq;

namespace GSD.Tests
{
	internal class MockContext
	{
		public MockContext()
		{
			SettingsRepoMock = new Mock<ISettingsRepository>();
			ProjectRepoMock = new Mock<IProjectRepository>();
		}

		public IProjectRepository ProjectRepo => ProjectRepoMock.Object;
		public Mock<IProjectRepository> ProjectRepoMock { get; }
		public ISettingsRepository SettingsRepo => SettingsRepoMock.Object;
		public Mock<ISettingsRepository> SettingsRepoMock { get; }
	}
}