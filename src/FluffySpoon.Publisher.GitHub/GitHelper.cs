using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FluffySpoon.Publisher.GitHub
{
  static class GitHelper
  {
    public static void Clone(string targetPath, Repository repository)
    {
      var programFilesPath = @"C:\Program Files";
      var gitPath = Path.Combine(
        programFilesPath,
        "Git",
        "bin",
        "git.exe");
      Directory.CreateDirectory(targetPath);
      CommandLineHelper.LaunchAndWait(new ProcessStartInfo(gitPath)
      {
        Arguments = "clone " + repository.CloneUrl,
        WorkingDirectory = Path.GetDirectoryName(targetPath)
      });
    }
  }
}
