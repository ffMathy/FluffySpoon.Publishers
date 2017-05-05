using FluffySpoon.Publisher.Local;
using FluffySpoon.Publisher.Remote;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.NuGet
{
  class NuGetPackageSystem : IRemotePackageSystem
  {
    public bool CanPublishPackage(ILocalPackage package)
    {
      return package is NuGetPackage;
    }

    public async Task<bool> DoesPackageWithVersionExistAsync(ILocalPackage package)
    {
      var nugetPackage = (NuGetPackage)package;
      return false;
    }

    public async Task UpsertPackageAsync(ILocalPackage package)
    {
    }
  }
}
