using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.NuGet
{
  class NuGetPackageScanner : ILocalPackageScanner
  {
    public async Task<IReadOnlyCollection<ILocalPackage>> ScanForPackagesInDirectoryAsync(string folderPath)
    {
      var packages = new HashSet<ILocalPackage>();

      var sourceDirectory = new DirectoryInfo(folderPath);
      if (!sourceDirectory.Exists)
        return packages;

      var solutionFilesInSourceDirectory = sourceDirectory
        .GetFiles("*.sln")
        .Where(x => x
          .Name
          .StartsWith($"{nameof(FluffySpoon)}."));

      return packages;
    }
  }
}
