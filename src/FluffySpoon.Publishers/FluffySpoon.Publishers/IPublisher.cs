using System.Threading.Tasks;

namespace FluffySpoon.Publishers
{
  public interface IPublisher
  {
    Task RefreshAllPackagesFromAllRepositoriesAsync();
  }
}