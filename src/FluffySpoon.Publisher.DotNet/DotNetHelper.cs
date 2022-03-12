using System.Diagnostics;

namespace FluffySpoon.Publisher.DotNet;

static class DotNetHelper
{
  public static void RestorePackages(string targetDirectory)
  {
    CommandLineHelper.LaunchAndWait(new ProcessStartInfo("dotnet")
    {
      Arguments = "restore",
      WorkingDirectory = targetDirectory
    });
  }

  public static void Test(string targetDirectory)
  {
    CommandLineHelper.LaunchAndWait(new ProcessStartInfo("dotnet")
    {
      Arguments = "test",
      WorkingDirectory = targetDirectory
    });
  }

  public static void Build(string targetDirectory)
  {
    CommandLineHelper.LaunchAndWait(new ProcessStartInfo("dotnet")
    {
      Arguments = "pack --output \"" + targetDirectory + "\" -c Release --include-source --include-symbols /p:ContinuousIntegrationBuild=true",
      WorkingDirectory = targetDirectory
    });
  }
}