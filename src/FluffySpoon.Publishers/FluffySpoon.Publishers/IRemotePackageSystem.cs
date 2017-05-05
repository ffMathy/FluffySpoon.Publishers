using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publishers
{
  interface IRemotePackageSystem
  {
    Task<bool> DoesPackageExistAsync(ILocalPackage package);

    Task UpdatePackage(ILocalPackage package);

    Task AddPackage(ILocalPackage package);
  }
}
