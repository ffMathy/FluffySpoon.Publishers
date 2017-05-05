using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publishers
{
  interface IRemoteSourceControlSystem
  {
    Task<IReadOnlyCollection<IRemoteSourceControlRepository>> GetAllRepositoriesAsync();
  }
}
