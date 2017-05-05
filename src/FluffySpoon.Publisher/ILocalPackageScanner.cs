using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher
{
  interface ILocalPackageScanner
  {
    Task<IReadOnlyCollection<ILocalPackage>> ScanForPackagesInDirectoryAsync(string folderPath);
  }
}
