using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher
{
  public interface IRemoteSourceControlSystem
  {
    Task<IReadOnlyCollection<IRemoteSourceControlRepository>> GetCurrentUserRepositoriesAsync();
  }
}
