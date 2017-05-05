using FluffySpoon.Publisher.Remote;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluffySpoon.Publisher
{
  public static class RegistrationExtensions
  {
    private static readonly HashSet<Type> SourceControlSystemTypes;

    static RegistrationExtensions()
    {
      SourceControlSystemTypes = new HashSet<Type>();
    }

    public static void AddSourceControlSystem<TSourceControlSystem>() where TSourceControlSystem : IRemoteSourceControlSystem
    {
      SourceControlSystemTypes.Add(typeof(TSourceControlSystem));
    }

    public static void AddRepositoryToPackagePublisher(this ServiceCollection services)
    {
      services.AddTransient<IRepositoryToPackagePublisher, RepositoryToPackagePublisher>();
      services.AddTransient<IEnumerable<IRemoteSourceControlSystem>>(provider =>
      {
        var sourceControlSystems = new HashSet<IRemoteSourceControlSystem>();
        foreach(var type in SourceControlSystemTypes)
        {
          sourceControlSystems.Add(
            (IRemoteSourceControlSystem) provider.GetRequiredService(type));
        }

        return sourceControlSystems;
      });
    }
  }
}
