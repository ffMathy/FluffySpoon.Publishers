using FluffySpoon.Publisher.DotNet;
using FluffySpoon.Publisher.Local;
using FluffySpoon.Publisher.Remote;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.NuGet;

class NuGetRemotePackageSystem : IRemotePackageSystem
{
    private readonly ISettings _repositoryFilter;
    private readonly INuGetSettings _nuGetSettings;

    public NuGetRemotePackageSystem(
        INuGetSettings nuGetSettings,
        ISettings repositoryFilter)
    {
        _nuGetSettings = nuGetSettings;
        _repositoryFilter = repositoryFilter;
    }

    public bool CanPublishPackage(ILocalPackage package)
    {
        return package is IDotNetLocalPackage &&
               package.PublishName.StartsWith(_repositoryFilter.ProjectPrefix) &&
               !package.PublishName.EndsWith(".Sample") &&
               !package.PublishName.EndsWith(".Tests");
    }

    public async Task<bool?> DoesPackageWithVersionExistAsync(ILocalPackage package)
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync($"https://api.nuget.org/packages/{package.PublishName.ToLowerInvariant()}.{package.Version}.nupkg");
            if (response.StatusCode != HttpStatusCode.NotFound && response.IsSuccessStatusCode)
            {
                return true;
            }
        }

        return false;
    }

    public async Task UpsertPackageAsync(ILocalPackage package)
    {
        Console.WriteLine("Upserting package " + package.PublishName + " version " + package.Version + " to remote package repository.");

        var basePath = Path.Combine(
            package.FolderPath,
            package.PublishName + "." + package.Version);
        
        await NuGetHelper.PublishAsync(
            basePath + ".nupkg",
            _nuGetSettings.ApiKey);

        await NuGetHelper.PublishAsync(
            basePath + ".symbols.nupkg",
            _nuGetSettings.ApiKey);

        package.PublishUrl = $"https://www.nuget.org/packages/{package.PublishName}/{package.Version}";
    }
}