using FluffySpoon.Publisher.Local;

namespace FluffySpoon.Publisher.NodeJs;

public interface INodeJsLocalPackage: ILocalPackage
{
  string PackageJsonFilePath { get; }
}