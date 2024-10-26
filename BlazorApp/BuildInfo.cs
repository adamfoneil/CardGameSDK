using Pekspro.BuildInformationGenerator;

namespace BlazorApp;

// sample commit

[BuildInformation(AddGitCommitId = true, AddLocalBuildTime = true, FakeIfDebug = false, FakeIfRelease = false, AddGitBranch = true)]
partial class BuildInfo
{
}
