using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FluffySpoon.Publishers.GitHub
{
  static class GitHelper
  {
    public static void Clone(string targetPath, Repository repository)
    {
      var information = new ProcessStartInfo("git.exe")
      {
        Arguments = "clone " + repository.CloneUrl,
        WorkingDirectory = targetPath
      };
      using (var process = Process.Start(information))
      {
        process.WaitForExit();
      }
    }
  }
}
