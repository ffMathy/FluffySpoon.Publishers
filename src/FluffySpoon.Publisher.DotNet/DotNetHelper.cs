using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FluffySpoon.Publisher.DotNet
{
  static class DotNetHelper
  {
    public static void RestorePackages(string targetDirectory)
    {
      var information = new ProcessStartInfo("dotnet.exe")
      {
        Arguments = "restore",
        WorkingDirectory = targetDirectory
      };
      using (var process = Process.Start(information))
      {
        process.WaitForExit();
      }
    }

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
