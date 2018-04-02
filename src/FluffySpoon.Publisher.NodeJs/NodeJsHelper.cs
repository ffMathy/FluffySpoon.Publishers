using System.Diagnostics;

namespace FluffySpoon.Publisher.DotNet
{
  static class NodeJsHelper
  {
    public static void RestorePackages(string targetDirectory)
    {
      CommandLineHelper.LaunchAndWait(new ProcessStartInfo("npm.cmd")
      {
        Arguments = "install",
        WorkingDirectory = targetDirectory
      });
    }

    public static void Build(string targetDirectory)
    {
      CommandLineHelper.LaunchAndWait(new ProcessStartInfo("tsc.cmd")
      {
        Arguments = "",
        WorkingDirectory = targetDirectory
      });
    }
  }
}
