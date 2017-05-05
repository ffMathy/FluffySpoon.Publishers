using System.Threading.Tasks;

namespace FluffySpoon.Publisher
{
  public interface IPackagePublisher
  {
    Task RefreshAllPackagesFromAllRepositoriesAsync();
  }
}