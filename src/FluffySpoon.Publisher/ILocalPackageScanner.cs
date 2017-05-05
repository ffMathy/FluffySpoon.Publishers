using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher
{
  public interface ILocalPackageScanner
  {
    Task<IReadOnlyCollection<ILocalPackage>> ScanForPackagesInDirectoryAsync(string folderPath);
  }
}
