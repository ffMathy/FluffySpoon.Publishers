using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FluffySpoon.Publishers.GitHub
{
  static class CommandLineHelper
  {
    public static void Build(string targetDirectory)
    {
      var information = new ProcessStartInfo("dotnet.exe")
      {
        Arguments = "pack --output \"" + targetDirectory + "\"",
        WorkingDirectory = targetDirectory
      };
      using (var process = Process.Start(information))
      {
        process.WaitForExit();
      }
    }
  }
}
