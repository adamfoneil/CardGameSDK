using Pekspro.BuildInformationGenerator;

namespace BlazorApp;

[BuildInformation(AddGitCommitId = true, AddLocalBuildTime = true, FakeIfDebug = false, FakeIfRelease = false, AddGitBranch = true)]
partial class BuildInfo
{
}
