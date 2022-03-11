using System.Threading.Tasks;

namespace FluffySpoon.Publisher;

public interface IRepositoryToPackagePublisher
{
  Task RefreshAllPackagesFromAllRepositoriesAsync();
}