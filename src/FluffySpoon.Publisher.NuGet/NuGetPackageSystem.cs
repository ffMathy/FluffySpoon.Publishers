using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.NuGet
{
  class NuGetPackageSystem : IRemotePackageSystem
  {
    public Task AddPackageAsync(ILocalPackage package)
    {
    }

    public Task<bool> DoesPackageExistAsync(ILocalPackage package)
    {
    }

    public Task UpdatePackageAsync(ILocalPackage package)
    {
    }
  }
}
