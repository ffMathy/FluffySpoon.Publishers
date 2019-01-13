using Octokit;
using System.Threading.Tasks;
using FluffySpoon.Publisher.Remote;
using System;
using FluffySpoon.Publisher.Local;
using System.Linq;

namespace FluffySpoon.Publisher.GitHub
{
	class GitHubSourceControlRepository : IGitHubSourceControlRepository
	{
		private readonly IGitHubClient _client;

		public string Name { get; set; }
		public string PublicUrl { get; set; }
		public string ContributeUrl { get; set; }
		public string Owner { get; set; }
		public string Summary { get; set; }

		public DateTime UpdatedAt { get; set; }

		public IRemoteSourceControlSystem System { get; set; }

		internal Repository GitHubClientRepository { get; set; }

		public GitHubSourceControlRepository(
		  IGitHubClient client)
		{
			_client = client;
		}

		public async Task DownloadToDirectoryAsync(string targetPath)
		{
			var repository = await _client.Repository.Get(Owner, Name);
			GitHelper.Clone(targetPath, repository);
		}

		public async Task RegisterPackageReleaseAsync(ILocalPackage package)
		{
			if(package.Version == null)
				return;

			if(package.PublishName == null)
				return;

			if(package.PublishUrl == null)
				return;

			var versionSlug = "v" + package.Version;
			var name = package.PublishName + "-" + versionSlug;

			var allReleases = await this._client.Repository.Release.GetAll(Owner, Name);

			foreach(var release in allReleases.Where(x => x.TagName != versionSlug))
				await this._client.Repository.Release.Delete(Owner, Name, release.Id);

			if(allReleases.Any(x => x.Name == name))
				return;

			await this._client.Repository.Release.Create(Owner, Name, new NewRelease(versionSlug) {
				Name = name,
				Body = $"Published automatically by https://github.com/ffMathy/FluffySpoon.Publishers." + 
					$"{Environment.NewLine}Repository link: {package.PublishUrl}"
			});
		}
	}
}
