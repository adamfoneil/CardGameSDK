using Pekspro.BuildInformationGenerator;

namespace BlazorApp;

// once more with feeling

[BuildInformation(AddGitCommitId = true, AddLocalBuildTime = true, FakeIfDebug = false, FakeIfRelease = false, AddGitBranch = true)]
partial class BuildInfo
{
}
