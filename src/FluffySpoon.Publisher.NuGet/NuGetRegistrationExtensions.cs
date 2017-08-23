using FluffySpoon.Publisher.NuGet;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace FluffySpoon
{
    public static class NuGetRegistrationExtensions
    {
        public static void AddNuGetProvider(
          this ServiceCollection services,
          string apiKey)
        {
            services.AddTransient<NuGetRemotePackageSystem>();
            services.AddTransient<INuGetSettings>(provider => new NuGetSettings(apiKey));

            RegistrationExtensions.AddPackageSystem<NuGetRemotePackageSystem>();
        }
    }
}
