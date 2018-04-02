using FluffySpoon.Publisher.DotNet;
using FluffySpoon.Publisher.Local;
using FluffySpoon.Publisher.Remote;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.NuGet
{
    class NpmRemotePackageSystem : IRemotePackageSystem
    {
        private readonly ISettings _repositoryFilter;
        private readonly INpmSettings _npmSettings;

        public NpmRemotePackageSystem(
            INpmSettings nuGetSettings,
            ISettings repositoryFilter)
        {
            _npmSettings = nuGetSettings;
            _repositoryFilter = repositoryFilter;
        }

        public bool CanPublishPackage(ILocalPackage package)
        {
			var folderPathName = Path.GetFileName(package.FolderPath);
            return package is INodeJsLocalPackage &&
			  folderPathName.StartsWith(_repositoryFilter.ProjectPrefix);
        }

        public async Task<bool?> DoesPackageWithVersionExistAsync(ILocalPackage package)
        {
			return null;
        }

        public async Task UpsertPackageAsync(ILocalPackage package)
        {
            Console.WriteLine("Upserting package " + package.PublishName + " version " + package.Version + " to remote package repository.");

            await NpmHelper.PublishAsync(
                package.FolderPath, 
                _npmSettings.AuthToken);
        }
    }
}
