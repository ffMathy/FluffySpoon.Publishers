using FluffySpoon.Publisher.Local;

namespace FluffySpoon.Publisher.DotNet
{
  class NodeJsLocalPackage : INodeJsLocalPackage
  {
    public string PublishName { get; set; }
    public string FolderPath { get; set; }
    public string PackageJsonFilePath { get; set; }
    public string Version { get; set; }

    public ILocalPackageProcessor Processor { get; set; }
  }
}