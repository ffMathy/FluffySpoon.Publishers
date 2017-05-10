using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FluffySpoon.Publisher
{
  public static class PathHelper
  {
    public static string GetFluffySpoonWorkingDirectory()
    {
      var drive = DriveInfo.GetDrives().First(x => x.DriveType == DriveType.Fixed && x.IsReady);
      var folderPath = Path.Combine(
        drive.RootDirectory.FullName,
        "FluffySpoon");
      return folderPath;
    }
  }
}
