using FluffySpoon.Publisher.Local;
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
        private static readonly HashSet<Type> PackageSystemTypes;
        private static readonly HashSet<Type> PackageProcessorTypes;

        static RegistrationExtensions()
        {
            SourceControlSystemTypes = new HashSet<Type>();
            PackageSystemTypes = new HashSet<Type>();
            PackageProcessorTypes = new HashSet<Type>();
        }

        public static void AddPackageProcessor<TPackageProcessor>() where TPackageProcessor : ILocalPackageProcessor
        {
            PackageProcessorTypes.Add(typeof(TPackageProcessor));
        }

        public static void AddPackageSystem<TPackageSystem>() where TPackageSystem : IRemotePackageSystem
        {
            PackageSystemTypes.Add(typeof(TPackageSystem));
        }

        public static void AddSourceControlSystem<TSourceControlSystem>() where TSourceControlSystem : IRemoteSourceControlSystem
        {
            SourceControlSystemTypes.Add(typeof(TSourceControlSystem));
        }

        public static void AddRepositoryToPackagePublisher(
            this ServiceCollection services,
            string repositoryPrefix)
        {
            services.AddTransient<IRepositoryToPackagePublisher, RepositoryToPackagePublisher>();
            services.AddTransient<ISettings>(provider => new Settings(repositoryPrefix));

            services.AddTransient(provider => GetServices<IRemoteSourceControlSystem>(provider, SourceControlSystemTypes));
            services.AddTransient(provider => GetServices<IRemotePackageSystem>(provider, PackageSystemTypes));
            services.AddTransient(provider => GetServices<ILocalPackageProcessor>(provider, PackageProcessorTypes));
        }

        private static IEnumerable<T> GetServices<T>(
          IServiceProvider provider,
          IEnumerable<Type> types)
        {
            var sourceControlSystems = new HashSet<T>();
            foreach (var type in types)
            {
                sourceControlSystems.Add(
                  (T)provider.GetRequiredService(type));
            }

            return sourceControlSystems;
        }
    }
}
