using Octokit;
using System.Diagnostics;
using System.IO;

namespace FluffySpoon.Publisher.GitHub;

static class GitHelper
{
  public static void Clone(string targetPath, Repository repository)
  {
    Directory.CreateDirectory(targetPath);
    CommandLineHelper.LaunchAndWait(new ProcessStartInfo("git")
    {
      Arguments = "clone " + repository.CloneUrl,
      WorkingDirectory = Path.GetDirectoryName(targetPath)
    });
  }
}