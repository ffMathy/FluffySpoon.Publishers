using FluffySpoon.Publisher.Local;
using System;

namespace FluffySpoon.Publisher.NuGet
{
  class NuGetPackage : ILocalPackage
  {
    public string PublishName { get; set; }
    public string FolderPath { get; set; }
    public string ProjectFilePath { get; set; }

    public int Version { get; set; }
  }
}