using FluffySpoon.Publisher.DotNet;
using FluffySpoon.Publisher.DotNet.Helpers;
using FluffySpoon.Publisher.NuGet;
using FluffySpoon.Publisher.Remote;
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
			
			services.AddTransient<IRemotePackageSystem, NuGetRemotePackageSystem>();
        }

		public static void AddDotNetNuGetSourceLinkProvider(
			this ServiceCollection services,
			string sourceLinkPackageName,
			string sourceLinkPackageVersion = null) 
		{
			services.AddTransient<IDotNetLocalPackagePreprocessor>(p => 
				new NuGetSourceLinkDotNetLocalPackagePreprocessor(
					p.GetRequiredService<IProjectFileParser>(),
					sourceLinkPackageName,
					sourceLinkPackageVersion));
		}
    }
}
