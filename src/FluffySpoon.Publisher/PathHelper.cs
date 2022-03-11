using System;
using System.IO;
using System.Linq;

namespace FluffySpoon.Publisher;

public static class PathHelper
{
  public static string GetFluffySpoonWorkingDirectory()
  {
    var folderPath = Path.Combine(
      Environment.CurrentDirectory,
      "FluffySpoon");
    return folderPath;
  }
}