using FluffySpoon.Publisher.Local;

namespace FluffySpoon.Publisher.DotNet
{
  public interface INodeJsLocalPackage: ILocalPackage
  {
    string PackageJsonFilePath { get; }
  }
}