using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher
{
  public interface IRemotePackageSystem
  {
    bool CanPublishPackage(ILocalPackage package);

    Task<bool> DoesPackageWithVersionExistAsync(ILocalPackage package);

    Task UpsertPackageAsync(ILocalPackage package);
  }
}
