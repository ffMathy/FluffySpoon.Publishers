using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FluffySpoon.Publisher.DotNet
{
  static class NuGetHelper
  {
    public static void Publish(string nugetPackageFile, string apiKey)
    {

      //nuget.exe setApiKey [your API key] -source https://www.nuget.org
      //nuget.exe push MyPackage.1.0.nupkg -Source https://www.nuget.org/api/v2/package

      var information = new ProcessStartInfo("nuget.exe")
      {
        Arguments = "setApiKey " + apiKey + " -source https://www.nuget.org",
        WorkingDirectory = //use fluffy spoon directory here and download nuget exe if not already there.
      };
      using (var process = Process.Start(information))
      {
        process.WaitForExit();
      }
    }
  }
}
