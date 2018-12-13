﻿using System.Diagnostics;

namespace FluffySpoon.Publisher.DotNet
{
  static class DotNetHelper
  {
    public static void RestorePackages(string targetDirectory)
    {
      CommandLineHelper.LaunchAndWait(new ProcessStartInfo("dotnet.exe")
      {
        Arguments = "restore",
        WorkingDirectory = targetDirectory
      });
    }

    public static void Build(string targetDirectory)
    {
      CommandLineHelper.LaunchAndWait(new ProcessStartInfo("dotnet.exe")
      {
        Arguments = "pack --output \"" + targetDirectory + "\" --include-symbols -p:SymbolPackageFormat=snupkg",
        WorkingDirectory = targetDirectory
      });
    }
  }
}
