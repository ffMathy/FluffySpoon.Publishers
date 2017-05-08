using FluffySpoon.Publisher.Local;

namespace FluffySpoon.Publisher.DotNet
{
  public interface IDotNetLocalPackage: ILocalPackage
  {
    string ProjectFilePath { get; }
  }
}