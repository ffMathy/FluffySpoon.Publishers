using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publishers
{
  class GitHubSourceControlSystem : IRemoteSourceControlSystem
  {
    public async Task<IReadOnlyCollection<IRemoteSourceControlRepository>> GetAllRepositoriesAsync()
    {
      return new[]
      {
        new GitHubSourceControlRepository()
        {
          Name = "FluffySpoon.AspNet"
        },
        new GitHubSourceControlRepository()
        {
          Name = "FluffySpoon.Angular.Http"
        },
        new GitHubSourceControlRepository()
        {
          Name = "FluffySpoon.Angular.Authentication.Jwt"
        }
      };
    }
  }
}
