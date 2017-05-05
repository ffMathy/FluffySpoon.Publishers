using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluffySpoon.Publishers
{
  public static class RegistrationExtensions
  {
    public static void AddPublishers(
      this ServiceCollection services)
    {
      services.AddTransient<IPublisher, Publisher>();
      services.AddSingleton<IEnumerable<IRemoteSourceControlSystem>>(p => new[]
      {
        new GitHubSourceControlSystem()
      });
    }
  }
}
