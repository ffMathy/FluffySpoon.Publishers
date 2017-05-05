using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.Local
{
  public interface ILocalPackageProcessor
  {
    Task<IReadOnlyCollection<ILocalPackage>> ScanForPackagesInDirectoryAsync(string folderPath);
    Task BuildPackageAsync(ILocalPackage package);
  }
}
