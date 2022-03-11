using System;
using System.IO;
using System.Linq;

namespace FluffySpoon.Publisher
{
  public static class PathHelper
  {
    public static string GetFluffySpoonWorkingDirectory()
    {
      var folderPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "FluffySpoon");
      return folderPath;
    }
  }
}
