using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.Remote
{
  public interface IRemoteSourceControlSystem
  {
    Task<IReadOnlyCollection<IRemoteSourceControlRepository>> GetCurrentUserRepositoriesAsync();
    Task<int> GetRevisionOfRepository(IRemoteSourceControlRepository repository);
  }
}
