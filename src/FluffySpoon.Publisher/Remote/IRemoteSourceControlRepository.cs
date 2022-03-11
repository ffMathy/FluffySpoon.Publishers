using System;
using System.Threading.Tasks;
using FluffySpoon.Publisher.Local;

namespace FluffySpoon.Publisher.Remote;

public interface IRemoteSourceControlRepository
{
	string Name { get; }
	string? PublicUrl { get; }
	string ContributeUrl { get; }
	string? Summary { get; }

	DateTime UpdatedAt { get; }

	IRemoteSourceControlSystem System { get; }

	Task DownloadToDirectoryAsync(string folderPath);

	Task RegisterPackageReleaseAsync(ILocalPackage package);
}