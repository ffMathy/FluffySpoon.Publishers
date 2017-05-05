using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher
{
  public interface IRemotePackageSystem
  {
    Task<bool> DoesPackageExistAsync(ILocalPackage package);

    Task UpdatePackageAsync(ILocalPackage package);

    Task AddPackageAsync(ILocalPackage package);
  }
}
