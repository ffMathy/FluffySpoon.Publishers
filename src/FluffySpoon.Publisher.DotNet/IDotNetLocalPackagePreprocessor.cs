using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.DotNet
{
  public interface IDotNetLocalPackagePreprocessor
  {
    Task PreprocessPackageAsync(IDotNetLocalPackage package);
  }
}
