using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.NuGet;

static class NuGetHelper
{
  public static async Task PublishAsync(string nugetPackageFile, string apiKey)
  {
    CommandLineHelper.LaunchAndWait(new ProcessStartInfo("dotnet")
    {
      Arguments = "nuget push \"" + nugetPackageFile + "\" -k " + apiKey + " -s https://api.nuget.org/v3/index.json"
    });
  }
}