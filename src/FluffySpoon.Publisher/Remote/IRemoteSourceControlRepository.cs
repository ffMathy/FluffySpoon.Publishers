using System.Threading.Tasks;

namespace FluffySpoon.Publisher.Remote
{
    public interface IRemoteSourceControlRepository
    {
        string Name { get; }
        string PublicUrl { get; }
		string ContributeUrl { get; }
		string Summary { get; }

        IRemoteSourceControlSystem System { get; }

        Task DownloadToDirectoryAsync(string folderPath);
    }
}