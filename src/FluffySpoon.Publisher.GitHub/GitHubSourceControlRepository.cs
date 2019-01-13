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
			var versionSlug = "v" + package.Version;

			var allReleases = await this._client.Repository.Release.GetAll(Owner, Name);

			if (package.Version == null)
				return;

			if (package.PublishName == null)
				return;

			if (package.PublishUrl == null)
				return;

			var descriptionLine = package.PublishName + ": " + package.PublishUrl;
			var existingRelease = allReleases.SingleOrDefault(x => x.TagName == versionSlug);
			if (existingRelease != null)
			{
				if(existingRelease.Body.Contains(descriptionLine))
					return;

				await this._client.Repository.Release.Edit(Owner, Name, existingRelease.Id, new ReleaseUpdate() {
					Body = existingRelease.Body + Environment.NewLine + descriptionLine
				});
			}
			else
			{
				await this._client.Repository.Release.Create(Owner, Name, new NewRelease(versionSlug)
				{
					Name = versionSlug,
					Body = $"Published automatically by https://github.com/ffMathy/FluffySpoon.Publishers." +
						Environment.NewLine + Environment.NewLine + descriptionLine
				});
			}
		}
	}
}
