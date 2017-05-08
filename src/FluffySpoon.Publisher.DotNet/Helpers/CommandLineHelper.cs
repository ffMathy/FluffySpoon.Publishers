using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FluffySpoon.Publisher.DotNet
{
  static class CommandLineHelper
  {
    public static void Build(string targetDirectory)
    {
      var information = new ProcessStartInfo("dotnet.exe")
      {
        Arguments = "pack --output \"" + targetDirectory + "\"",
        WorkingDirectory = targetDirectory,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        RedirectStandardInput = true
      };
      using (var process = Process.Start(information))
      {
        process.WaitForExit();
      }
    }
  }
}
