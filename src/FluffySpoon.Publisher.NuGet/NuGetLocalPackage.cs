using FluffySpoon.Publisher.Local;
using System;

namespace FluffySpoon.Publisher.NuGet
{
  class NuGetLocalPackage : ILocalPackage
  {
    public string PublishName { get; set; }
    public string FolderPath { get; set; }
    public string ProjectFilePath { get; set; }
    public string Version { get; set; }

    public ILocalPackageProcessor Processor { get; set; }
  }
}