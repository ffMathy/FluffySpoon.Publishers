using System.Threading.Tasks;
using FluffySpoon.Publisher.Local;

namespace FluffySpoon.Publisher.Remote
{
  public interface IRemotePackageSystem
  {
    bool CanPublishPackage(ILocalPackage package);

    Task<bool> DoesPackageWithVersionExistAsync(ILocalPackage package);

    Task UpsertPackageAsync(ILocalPackage package);
  }
}
