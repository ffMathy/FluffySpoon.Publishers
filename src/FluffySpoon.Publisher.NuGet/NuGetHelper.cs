using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.DotNet
{
  static class NuGetHelper
  {
    public static async Task PublishAsync(string nugetPackageFile, string apiKey)
    {
      var nugetExePath = Path.Combine(
        PathHelper.GetFluffySpoonWorkingDirectory(),
        "nuget.exe");
      await DownloadNuGetExecutableIfNotExistsAsync(nugetExePath);

      CommandLineHelper.LaunchAndWait(new ProcessStartInfo(nugetExePath)
      {
        Arguments = "setApiKey " + apiKey + " -source https://www.nuget.org",
        RedirectStandardOutput = true
      });
      
      CommandLineHelper.LaunchAndWait(new ProcessStartInfo(nugetExePath)
      {
        Arguments = "push \"" + nugetPackageFile + "\" -source https://www.nuget.org/api/v2/package"
      });
    }

    private static async Task DownloadNuGetExecutableIfNotExistsAsync(string nugetPath)
    {
      if (File.Exists(nugetPath))
        return;

      using (var client = new HttpClient())
      {
        var bytes = await client.GetByteArrayAsync("https://dist.nuget.org/win-x86-commandline/latest/nuget.exe");
        File.WriteAllBytes(nugetPath, bytes);
      }
    }
  }
}
