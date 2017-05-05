using System.Threading.Tasks;

namespace FluffySpoon.Publishers
{
  public interface IPackagePublisher
  {
    Task RefreshAllPackagesFromAllRepositoriesAsync();
  }
}