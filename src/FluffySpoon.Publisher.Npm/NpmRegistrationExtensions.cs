using FluffySpoon.Publisher.NuGet;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace FluffySpoon
{
    public static class NpmRegistrationExtensions
    {
        public static void AddNpmProvider(
          this ServiceCollection services,
          string authToken)
        {
            services.AddTransient<NpmRemotePackageSystem>();
            services.AddTransient<INpmSettings>(provider => new NpmSettings(authToken));

            RegistrationExtensions.AddPackageSystem<NpmRemotePackageSystem>();
        }
    }
}
